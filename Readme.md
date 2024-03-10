# Rexname (regex-rename)
A tool for using RegEx to bulk rename files. 

## Installation
1. Clone or Download repo 
2. Using dotnet 8 or newer run the following commands
   1. dotnet pack rexname.csproj
   2. dotnet tool install --global --add-source ./nupkg rexname

## Uninstall
1. Using dotnet cli run the following commands
   1. dotnet tool uninstall --global rexname

## Usage
The rexname tool runs on the command line and you can access it's help information with the following command
```
> rexname --help

Description:
  Rename all files in the directory matching the given pattern. Can use match groups in the replacement name

Usage:
  rexname [options]

Options:
  --pattern <pattern>  RegEx matching pattern
  --name <name>        Replacement name pattern
  --dry                mark this as a dry run or not [default: False]
  --version            Show version information
  -?, -h, --help       Show help and usage information
```

First specify a file selection pattern. Only files matching this pattern will be renamed. For instance if we wanted to rename all mp4 video files one possible pattern would be `(?<name>.*)\.mp4$`. Then you need to specify a renaming template something like maybe we want to append ".old" to the end of each file then the template would be `{name}.mp4.old`. Arguments in {} will expand to the value of capture groups from the original matching pattern. 

The combined CLI instruction would be 
```
> rexname --pattern "(?<name>.*)\.mp4$" --name "{name}.mp4.old"
```

An optional parameter `--dry true` allows one to see the changes that would occur as a result of renaming without actually renaming the files. Good for testing that your pattern or name template are correct without causing permanent damage. 

```
> rexname --pattern "(?<name>.*)\.mp4$" --name "{name}.mp4.old" --dry true
```