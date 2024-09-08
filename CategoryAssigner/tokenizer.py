import re


def tokenize(script_args : list[str]) -> list[str]:
    if len(script_args) < 1:
        print("No arguments were given!")
        return SyntaxError

    args_string = ' '.join(script_args)

    pattern = r"--\S+"
    match = re.search(pattern=pattern, string=args_string)

    language = match.group()[2:] if match != None else ''

    food = ' '.join(script_args[:len(script_args) - 1]) if language != '' else ' '.join(script_args)

    return [food, language]
