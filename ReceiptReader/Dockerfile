FROM python:3.10

WORKDIR /app

RUN git clone https://github.com/ultralytics/yolov5 

RUN pip install --no-cache-dir -r yolov5/requirements.txt

COPY best2.pt . 
COPY model_runner.py . 

ENTRYPOINT ["python", "model_runner.py"]