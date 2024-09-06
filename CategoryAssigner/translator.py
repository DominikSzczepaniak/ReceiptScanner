import googletrans as tr

translator = tr.Translator()


def translate(txt : str, dst : str, src : str) -> str:
    translated : str = translator.translate(txt, dest=dst, src=src).text

    return translated
