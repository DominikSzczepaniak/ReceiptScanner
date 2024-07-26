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

ap = argparse.ArgumentParser()
ap.add_argument("--i", "--image", required=True, help="path to input image")
ap.add_argument("--d", "--debug", type=int, default=-1, help="whether or not to show extra debug information")
args = vars(ap.parse_args())

if(args["i"][-5:] == ".HEIC"):
    heif_file = pillow_heif.read_heif(args["i"])
    image = PIL.Image.frombytes(
        heif_file.mode, 
        heif_file.size, 
        heif_file.data,
        "raw",
        heif_file.mode,
        heif_file.stride,
    )

    image.save(args["i"][0:-5] + ".jpg")
    os.remove(args["i"])

    args["i"] = args["i"][0:-5] + ".jpg"

orig = cv2.imread(args["i"])
image = orig.copy()
image = imutils.resize(image, width=500)
ratio = orig.shape[1] / float(image.shape[1])

gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
blurred = cv2.GaussianBlur(gray, (5, 5), 0)
edged = cv2.Canny(blurred, 75, 200)

if args["d"] > 0:
    cv2.imshow("Input", image)
    cv2.imshow("Edged", edged)
    cv2.waitKey(0)

ret, thresh = cv2.threshold(edged.copy(), 127, 255, 0)
cnts, abc = cv2.findContours(thresh, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
# cnts = imutils.grab_contours(cnts)
cnts = sorted(cnts, key=cv2.contourArea, reverse=True)
# output = image.copy()
# cv2.drawContours(output, cnts, 99, (0, 255, 0), 2)
# cv2.imshow("Contours", output)
# cv2.waitKey(0)
# exit(0)

receiptCnt = None
for c in cnts:
    peri = cv2.arcLength(c, True)
    approx = cv2.approxPolyDP(c, 0.02 * peri, True)
    if(len(approx) == 4):
        receiptCnt = approx
        break

if receiptCnt is None:
    raise Exception("Could not find receipt outline")

if args["d"] > 0:
    output = image.copy()
    cv2.drawContours(output, [receiptCnt], -1, (0, 255, 0), 2)
    cv2.imshow("Receipt outline", output)
    cv2.waitKey(0)

receipt = four_point_transform(orig, receiptCnt.reshape(4, 2) * ratio)

cv2.imshow("Receipt", imutils.resize(receipt, width=500))
cv2.waitKey(0)


options = "--psm 6"
language = "pol"
os.environ['TESSDATA_PREFIX'] = r'C:\Users\szcze\AppData\Local\Programs\Tesseract-OCR\tessdata'
pytesseract.pytesseract.tesseract_cmd = r'C:\Users\szcze\AppData\Local\Programs\Tesseract-OCR/tesseract.exe'

text = pytesseract.image_to_string(cv2.cvtColor(receipt, cv2.COLOR_BGR2RGB), config=options, lang=language)

print(text)
print()
print()
pattern = r'([0-9]+\,[0-9]+)'

suma = r'SUM.'
for row in text.split('\n'):
    if(re.search(pattern, row)):
        print(row)