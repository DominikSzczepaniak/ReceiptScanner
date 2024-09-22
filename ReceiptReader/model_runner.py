import subprocess
import os
import argparse
from PIL import Image
from imutils.perspective import four_point_transform
import PIL.Image
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
import PIL
import pytesseract
import os
import pillow_heif
import cv2
import imutils
import re 
import argparse
import scipy
from imutils.perspective import four_point_transform

def detect(image_path, image_name, output_folder):
    subprocess.run(f"python yolov5/detect.py --weights best2.pt --img 640 --conf 0.15 --source {image_path} --project output --name {output_folder}")

def get_highest_file_number(directory):
    id = 0
    for filename in os.listdir(directory):
        id = max(id, int(filename))
    return id

def crop_to_bounding_box(image_path, rgb_color=(6, 41, 253)):
    image = cv2.imread(image_path)
    
    hsv_image = cv2.cvtColor(image, cv2.COLOR_BGR2HSV)
    
    color_bgr = np.uint8([[rgb_color]])
    color_hsv = cv2.cvtColor(color_bgr, cv2.COLOR_RGB2HSV)[0][0]

    lower_bound = np.array([color_hsv[0] - 10, 50, 50])
    upper_bound = np.array([color_hsv[0] + 10, 255, 255])

    mask = cv2.inRange(hsv_image, lower_bound, upper_bound)
    contours, _ = cv2.findContours(mask, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)

    if len(contours) == 0:
        print("Nie znaleziono ramki o podanym kolorze.")
        return image

    largest_contour = max(contours, key=cv2.contourArea)
    x, y, w, h = cv2.boundingRect(largest_contour)

    cropped_image = image[y:y+h, x:x+w]

    return cropped_image

def convertToNumeric(text):
    beforeComma, afterComma = "", ""
    for i in range(len(text)):
        if(text[i] == ','):
            beforeComma = text[0:i]
            afterComma = text[i+1:]
            break
    result = beforeComma + '.' + afterComma
    return float(result)

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Deep Learning Model")
    parser.add_argument("image_path", type=str, help="Ścieżka do pliku obrazu")
    args = parser.parse_args()
    
    filename = args.image_path
    image_path = f"images/{filename}"
    output_folder = get_highest_file_number("output/") + 1
    detect(image_path, filename, output_folder)
    output_path = f"output/{output_folder}/{filename}"
    img = crop_to_bounding_box(output_path)
    # four_point_transform(img, np.array([[0, 0], [0, 100], [100, 100], [100, 0]]))
    # cv2.imshow("done", img)
    # cv2.waitKey(0)
    options = "--psm 6"
    language = "pol"
    os.environ['TESSDATA_PREFIX'] = r'C:\Users\szcze\AppData\Local\Programs\Tesseract-OCR\tessdata'
    pytesseract.pytesseract.tesseract_cmd = r'C:\Users\szcze\AppData\Local\Programs\Tesseract-OCR/tesseract.exe' #TODO change to docker directory

    text = pytesseract.image_to_string(cv2.cvtColor(img, cv2.COLOR_BGR2RGB), config=options, lang=language)

    print(text)

    #TODO extract data from text and send it to api
