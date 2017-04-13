#!/usr/bin/perl

# Usage: $0 <num-k-rows>
# e.g: $0 350

use warnings;
use strict;

my $database = $ENV{'database'};
my $table = $ENV{'table'};

my @dims = ('A'..'Z');
my $from = 0;
#my $to = $from + 5000 * 70;
my $to = 1000 * $ARGV[0];
my @ids = ($from..$to-2);

my ($ss,$mm,$hh,) = localtime(time);


#
# Inserts rows
#
print "INSERT INTO $database.$table (c_date, step, position, mo, fun_name, min, measure, max, dim, info, spent_time, checkv)\n";
print "VALUES\n";
foreach (@ids) {
	my $dim = $dims[$_%26];
	my $min = $_ % 60;
	my $checkv = $_ % 2 ?-1 : -2;
	($ss,$mm,$hh,) = localtime(time);
	$ss = sprintf("%02d", $ss);
	$mm = sprintf("%02d", $mm);
	$hh = sprintf("%02d", $hh);

	print "('REPLACE_ME $hh:$mm:$ss', 1660,2000200,'A058_GND_ICPS0_$_','DCV_EVEN',0.005,0.016,2,'$dim','OK',99,$checkv),\n";

}

print "('REPLACE_ME $hh:$mm:$ss', 1660,2000200,'A058_GND_ICPS0_XXX','DCV_EVEN',0.005,0.016,2,'Z','OK',99,-3);\n";




