#!/bin/bash

# Set 'error' page to downtime version when build is ongoing
if [  $# -eq 0 ]; then
    echo "Must run script w/ one arg, either 'start' or 'end'"
    exit 1
fi

rm static/error.html

if [ "$1" == "start" ]; then
    echo "Set error page to downtime."
    cp static/downtime.html static/error.html
else
    echo "Set error page back to 404 mode."
    cp static/404.html static/error.html
fi