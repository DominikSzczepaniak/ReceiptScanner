import sys
import tokenizer as tk
import model as md
import translator as tr


def categorize() -> str:
    args = sys.argv[1:]


    # Tokenize the input
    input_data : list[str] = tk.tokenize(args)

    food : str = input_data[0]
    language : str = input_data[1]


    # Translate input to English (if neccessary)
    if language != '':
        food = tr.translate(food, 'en', language)


    # Categorize the food # TODO
    category : str = md.resolve(food)


    # Translate back to selected language
    if language != '' and category != '':
        category = tr.translate(category, 'en', language)
    

    # Debugging area
    print(f'Food: {food}')
    print(f'Language: {language}')
    print(f'Category: {category}')


    # Return correct food category
    return category




if __name__ == '__main__':
    print(categorize())
