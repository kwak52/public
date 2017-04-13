#!/bin/sh

cd /mnt/home2/
chcon -R system_u:object_r:mysqld_db_t:s0 mysql
