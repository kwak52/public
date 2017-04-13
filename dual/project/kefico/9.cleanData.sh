#!/bin/sh

cat << EOF | mysql

use kefico;

delete from dynamicTopSummary;
delete from staticTopSummary;
delete from staticDailyStepSummary;
delete from measure;
delete from pdv;
delete from user;
delete from step;
delete from bundle;
delete from pdvGroup;
delete from pdvTestList;

delete from dimension;
delete from preference;
delete from tableRevision;
delete from ccs;
delete from function;


delete from log;


ALTER TABLE dynamicTopSummary
	DROP COLUMN duration_gc
	, DROP COLUMN percentGood_gc
	, DROP COLUMN percentGood100_gc
	, DROP COLUMN lastECUs_gc
;

EOF


