#!/bin/sh

setenforce 0

echo never > /sys/kernel/mm/transparent_hugepage/enabled
echo never > /sys/kernel/mm/transparent_hugepage/defrag

ps_tokudb_admin --enable -ukwak

systemctl restart mysqld
