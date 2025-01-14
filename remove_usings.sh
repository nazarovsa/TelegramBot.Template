#!/bin/bash

# Create a temporary file
temp_file="codebase_temp.md"

# Process the file
awk '
BEGIN { in_code_block = 0 }
{
    if ($0 ~ /^```/) {
        in_code_block = !in_code_block
        print
        next
    }
    
    if (in_code_block) {
        if (!($0 ~ /^using /)) {
            print
        }
    } else {
        print
    }
}' codebase.md > "$temp_file"

# Replace original file with cleaned version
mv "$temp_file" codebase.md
