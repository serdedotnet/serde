#!/bin/bash

set -u
set -e

usage() { echo "Usage: $0 [--pack]" 1>&2; exit 1; }

pack=false
props=''

while [[ $# > 0 ]]; do
    opt="$(echo "$1" | tr "[:upper:]" "[:lower:]")"
    case "$opt" in
        --help|-h)
            usage
            exit 0
            ;;
        --pack)
            pack=true
            ;;
        *)
            props="$props $1"
            ;;
    esac
    shift
done

if [ "$pack" = true ]; then
    for f in src/pack/*.proj; do dotnet pack -warnaserror -c Release $f $props; done
else
    dotnet build -warnaserror serde-dn.sln $props
fi