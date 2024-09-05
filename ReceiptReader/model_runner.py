import subprocess
import os
import argparse
from PIL import Image

def detect(image_path, image_name, output_folder):
    subprocess.run(f"python yolov5/detect.py --weights best2.pt --img 640 --conf 0.15 --source {image_path} --project output --name {output_folder}")

def get_highest_file_number(directory):
    id = 0
    for filename in os.listdir(directory):
        id = max(id, int(filename))
    return id

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Deep Learning Model")
    parser.add_argument("image_path", type=str, help="Ścieżka do pliku obrazu")
    args = parser.parse_args()
    
    filename = args.image_path
    image_path = f"images/{filename}"
    output_folder = get_highest_file_number("output/") + 1
    detect(image_path, filename, output_folder)
    print("Done")