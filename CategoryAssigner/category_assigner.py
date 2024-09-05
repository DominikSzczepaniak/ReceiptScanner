import sys
import tokenizer as tk
import model as md


def categorize() -> str:
    args = sys.argv[1:]

    # Tokenize the input
    input_data : list[str] = tk.tokenize(args)

    # Translate input to English (if neccessary)
    # TODO

    # Categorize the food
    # TODO

    # Translate back to selected language
    # TODO

    # Return correct food category
    # TODO
    return ''




if __name__ == '__main__':
    print(categorize())
