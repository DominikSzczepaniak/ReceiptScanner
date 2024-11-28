Packages:
Tesseract OCR
Pillow
CV2
Numpy
Scipy
Imutils
pytesseract
argparse


Installation guide:
1. cd ReceiptReader && git clone https://github.com/ultralytics/yolov5
2. pip install -r yolov5/requirements
3. cd ../Frontend && npm install 
4. cd ../Backend && touch appsettings.Development.json
5. Create local postgres database. 
6. Run database/initialization files in order
7. In Backend/appsettings.Development.json add:
```
{
  "ConnectionString": "Host=;Port=;Database=;Username=;Password=;"
}
```
With your data.

8. Go to Frontend directory and run npm run dev
9. Go to Backend directory and run project 
10. Done