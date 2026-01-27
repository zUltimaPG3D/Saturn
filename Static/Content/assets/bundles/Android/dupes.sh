#!/bin/bash

for file in *; do
    if [[ -f "$file" ]]; then
        if find . -mindepth 2 -type f -name "$file" | grep -q .; then
            echo "rm $file"
            rm "$file"
        fi
    fi
done

echo "done"
