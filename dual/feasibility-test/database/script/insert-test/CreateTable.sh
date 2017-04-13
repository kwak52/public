#!/bin/sh

#
# partition type hash 는 drop partition 이 안된다.   DROP PARTITION can only be used on RANGE/LIST partitions
# date 를 이용한 hash partition 은 very bad idea!!  http://stackoverflow.com/questions/6093585/how-to-partition-a-table-by-datetime-column
#



mydir="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

if [ "x$database" = "x" -o "x$table" = "x" -o "x$mysql" = "x" ]; then
  source $mydir/../setenv
fi

#
# Generates partition informations
#
function generate_partitions()
{
  counter=0
  daysInYear=365
  delimiter=','
  while [ $counter -le $daysInYear ]; do
    if [ $counter = $daysInYear ]; then
      delimiter=''
    fi
    today=$(date +"%Y%m%d" -d "2016-01-01+$counter days")
    tomorrow=$(date +"%Y-%m-%d" -d "$today+1 days")
    echo "  PARTITION p$today VALUES LESS THAN ( TO_DAYS('$tomorrow') )$delimiter"
    let counter=counter+1
  done
}

cat << EOF | $mysql

CREATE DATABASE IF NOT EXISTS $database;
USE $database;

CREATE TABLE IF NOT EXISTS $table (
        id BIGINT AUTO_INCREMENT,
        -- ts TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
        c_date DATETIME, -- DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP, 
	-- c_time TIME,
        step INT,	-- 실제로는 NOT INT.  e.g "1021.1"
        position INT,
        mo VARCHAR(32),  -- mod(module?) name
        fun_name VARCHAR(32),
        min FLOAT,
        measure FLOAT,
        max FLOAT,
        dim CHAR(8),
        info VARCHAR(32),
        spent_time INT,
        checkv INT,
	PRIMARY KEY pri(c_date, id),
	INDEX dim(dim),
	INDEX info(info),
	INDEX id_dim(id, dim)
/*
	KEY(id),
	INDEX dim(dim, info)
*/

)
-- ENGINE=TokuDB

-- http://jason-heo.github.io/mysql/2014/03/05/innodb-barracuda.html
ENGINE=InnoDB ROW_FORMAT=COMPRESSED KEY_BLOCK_SIZE=4

-- http://stackoverflow.com/questions/19355024/mysql-table-partition-by-month
PARTITION BY RANGE( TO_DAYS(c_date) )
(
  -- PARTITION pmin VALUES LESS THAN ( TO_DAYS('2016-01-01') ),
`generate_partitions`
  -- PARTITION pmax VALUES LESS THAN MAXVALUE
);


CREATE TABLE IF NOT EXISTS statistics (
 ts TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
 elapsed INT,
 dbsize INT,
 num_rows BIGINT
);

EOF
