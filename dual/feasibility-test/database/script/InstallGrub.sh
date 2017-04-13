#!/bin/sh


cp /boot/grub2/grub.cfg /boot/grub2/grub.cfg.orig
cp 15_Windows /etc/grud.d/15_Windows
grub2-mkconfig -o /boot/grub2/grub.cfg   
