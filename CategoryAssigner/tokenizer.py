import re


def tokenize(script_args : list[str]) -> list[str]:
    if len(script_args) < 1:
        print("No arguments were given!")
        return SyntaxError

    # Take out script language mode
    args_string : str = ' '.join(script_args)

    pattern : str = r"--\S+"
    match = re.search(pattern=pattern, string=args_string)

    language : str = match.group()[2:] if match != None else ''

    # Get food name
    food : str = ' '.join(script_args[:len(script_args) - 1]) if language != '' else ' '.join(script_args)

    # Return tokenized data
    return [food, language]
