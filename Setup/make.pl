#!/usr/bin/perl

use strict;
use warnings;

use XML::Simple;
use Time::Piece;
use Data::Dumper;

my $DEBUG = 1;
my @BUILD = ( 'win-x64', 'linux-x64' );
my $BUILD_PROBS = '../osFotoFix/Directory.Build.props';
my $NSIS = '/usr/bin/makensis';

sub main()
{
  my $version = UpdateVersion();

  foreach my $build (@BUILD)
  {
    BuildTarget( $build );
    if( $build =~ 'win10' ) {
      MakeWindowsSetup( $build, $version );
    }
    if( $build =~ 'linux' ) {
      MakeLinuxSetup( $build, $version );
    }
  }
}

sub UpdateVersion()
{
  print "------------------------------------------------------------------------------------\n";
  print "update version\n";

  my $xml = XMLin( $BUILD_PROBS, (KeepRoot => 0, NoAttr => 1)  );
  print Dumper $xml if $DEBUG;

  my $version = $xml->{PropertyGroup}->{Version};
  print "  old version: $version\n" if $DEBUG;
  my ($major, $minjor, $release, $build) = split( /\./, $version );

  $release++;
  my $t = localtime;
  $build = sprintf( "%d%02d%d", $t->yy, $t->week, $t->day_of_week +1 );
  $version = join( '.', ( $major, $minjor, $release, $build ) );
  print "  create new version: $version\n";

  print "  update $BUILD_PROBS\n";
  $xml->{PropertyGroup}->{Version} = $version;
  $xml->{PropertyGroup}->{FileVersion} = $version;
  $xml->{PropertyGroup}->{InformationalVersion} = "${version}_make_by_os";

  open( FH, '>', $BUILD_PROBS ) or die $!;
  print FH XMLout( $xml, (RootName => 'Project', NoAttr => 1) );
  close( FH );

  return $version;
}

sub BuildTarget($)
{
  my $target = shift; 

  print "------------------------------------------------------------------------------------\n";
  print "build and create setup for windows\n";

  if ( -d "$target-deployment" ) {
    print "remove $target-deployment\n";
    `rm -r "$target-deployment"`;
  }

  print "run dotnet publish\n";
  my $build_args = "../osFotoFix -o $target-deployment -r $target -p:PublishReadyRun=true -p:PublishSingleFile=false -p:PublishedTrim=true --self-contained true";
  print "$build_args\n" if $DEBUG;
  #`dotnet publish $build_args`;
  my $status = system( "dotnet publish $build_args" );
  die "Faild to build\n" if( ($status >>=8) != 0);

  `cp ../osFotoFix/osFotoFix.png $target-deployment`;
  print "Build OK\n";

}

sub MakeWindowsSetup($$)
{
  my ($target, $version) = @_;

  if ( ! -d "$target-setup" ) {
    print "create $target-setup\n";
    `mkdir $target-setup`;
  }

  print "run makensis\n";
  `$NSIS -DVERSION=$version $target.nsi`;

  print "OK\n";
  print "------------------------------------------------------------------------------------\n";
}

sub MakeLinuxSetup($$)
{
  my ($target, $version) = @_;

  if ( ! -d "$target-setup" ) {
    print "create $target-setup\n";
    `mkdir $target-setup`;
  }

  `rm -r $target-setup/`;
  `mkdir -p $target-setup/osfotofix/DEBIAN`;

  open( my $fh_in, "<", "DEBIAN/control" );
  open( my $fh_out, ">", "$target-setup/osfotofix/DEBIAN/control" );
  while(<$fh_in>) {
    chomp;
    $_ =~ s/^Version.*$/Version: $version/;
    print $fh_out "$_\n";
  }
  close $fh_in;
  close $fh_out;

  `mkdir -p $target-setup/osfotofix/opt/osfotofix`;
  `cp $target-deployment/* $target-setup/osfotofix/opt/osfotofix/`;
  `mkdir -p $target-setup/osfotofix/usr/share/applications`;
  `cp osfotofix.desktop $target-setup/osfotofix/usr/share/applications/`;
  `dpkg-deb --build $target-setup/osfotofix`;

  print "OK\n";
  print "------------------------------------------------------------------------------------\n";
}

main();
