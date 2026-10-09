

import os
import subprocess
import xml.etree.ElementTree as ET
from PIL import Image

EDGE_PATH = r"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"

def render_html_to_png(html_content, output_png, width=1200, height=800):
    abs_png = os.path.abspath(output_png)
    temp_html = abs_png.replace(".png", "_temp.html")
    
    with open(temp_html, "w", encoding="utf-8") as f:
        f.write(html_content)
        
    cmd = [
        EDGE_PATH,
        "--headless=new",
        "--disable-gpu",
        f"--window-size={width},{height}",
        f"--screenshot={abs_png}",
        f"file:///{os.path.abspath(temp_html)}"
    ]
    subprocess.run(cmd, capture_output=True, text=True)
    
    if os.path.exists(temp_html):
        os.remove(temp_html)
        
    if os.path.exists(abs_png):
        try:
            im = Image.open(abs_png)
            bg = Image.new(im.mode, im.size, (255, 255, 255)
            print(f"Rendered: {output_png} ({im.size[0]}x{im.size[1]})")
        except Exception as e:
            print(f"Image processing note: {e}")
    else:
        print(f"Failed to render: {output_png}")

print("Helper ready")
