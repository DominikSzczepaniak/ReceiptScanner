import sys
import tokenizer as tk
import model as md
import translator as tr


def categorize() -> str:
    args = sys.argv[1:]

    input_data = tk.tokenize(args)

    food = input_data[0]
    language = input_data[1]

    if language != '':
        food = tr.translate(food, 'en', language)

    # load data frames etc. TODO

    category = md.resolve(food)

    if language != '' and category != '':
        category = tr.translate(category, 'en', language)

    return category


if __name__ == '__main__':
    print(categorize())
