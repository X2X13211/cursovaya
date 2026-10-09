# -*- coding: utf-8 -*-
"""
Script to generate all diagrams and markdown documents for Practical Work 1 & 2.
Student: Vitenik P.L. (Витеник П.Л.)
Topic: Dormitory Management System (Система управления общежитием)
"""

import os
import subprocess
from PIL import Image

EDGE_PATH = r"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"

def render_html_to_png(html_content, output_png, width=1350, height=880):
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
    res = subprocess.run(cmd, capture_output=True, text=True)
    if os.path.exists(temp_html):
        os.remove(temp_html)
        
    if os.path.exists(abs_png):
        print(f"[OK] Generated image: {output_png} ({os.path.getsize(abs_png)} bytes)")
    else:
        print(f"[ERROR] Failed to generate: {output_png}, err: {res.stderr}")

def create_drawio_file(filename, cells_xml, width=1350, height=900):
    xml_content = f'''<mxfile host="app.diagrams.net" modified="2026-10-09T22:30:00.000Z" agent="Mozilla/5.0" version="24.0.0" type="device">
  <diagram id="diag_1" name="Page-1">
    <mxGraphModel dx="{width}" dy="{height}" grid="1" gridSize="10" guides="1" tooltips="1" connect="1" arrows="1" fold="1" page="1" pageScale="1" pageWidth="{width}" pageHeight="{height}" math="0" shadow="0">
      <root>
        <mxCell id="0" />
        <mxCell id="1" parent="0" />
{cells_xml}
      </root>
    </mxGraphModel>
  </diagram>
</mxfile>'''
    os.makedirs(os.path.dirname(filename), exist_ok=True)
    with open(filename, "w", encoding="utf-8") as f:
        f.write(xml_content)
    print(f"[OK] Saved draw.io file: {filename}")

# ==============================================================================
# 1. USE CASE DIAGRAM
# ==============================================================================
def generate_usecase():
    drawio_path = "docs/model/usecase.drawio"
    png_path = "docs/model/usecase.png"
    
    cells = '''
        <mxCell id="sys_box" value="Система управления общежитием" style="shape=rect;html=1;whiteSpace=wrap;fillColor=#F8FAFC;strokeColor=#475569;strokeWidth=2;align=left;verticalAlign=top;spacingLeft=20;spacingTop=15;fontStyle=1;fontSize=15;fontColor=#1E293B;dashed=0;rounded=1;arcSize=4;" vertex="1" parent="1">
          <mxGeometry x="240" y="30" width="760" height="790" as="geometry" />
        </mxCell>

        <mxCell id="act_student" value="Студент" style="shape=umlActor;verticalLabelPosition=bottom;verticalAlign=top;html=1;outlineConnect=0;fillColor=#EFF6FF;strokeColor=#2563EB;strokeWidth=2;fontSize=13;fontStyle=1;fontColor=#1E3A8A;" vertex="1" parent="1">
          <mxGeometry x="70" y="160" width="45" height="80" as="geometry" />
        </mxCell>
        <mxCell id="act_warden" value="Комендант" style="shape=umlActor;verticalLabelPosition=bottom;verticalAlign=top;html=1;outlineConnect=0;fillColor=#ECFDF5;strokeColor=#059669;strokeWidth=2;fontSize=13;fontStyle=1;fontColor=#064E3B;" vertex="1" parent="1">
          <mxGeometry x="70" y="440" width="45" height="80" as="geometry" />
        </mxCell>

        <mxCell id="act_tutor" value="Воспитатель" style="shape=umlActor;verticalLabelPosition=bottom;verticalAlign=top;html=1;outlineConnect=0;fillColor=#FEF3C7;strokeColor=#D97706;strokeWidth=2;fontSize=13;fontStyle=1;fontColor=#78350F;" vertex="1" parent="1">
          <mxGeometry x="1090" y="180" width="45" height="80" as="geometry" />
        </mxCell>
        <mxCell id="act_admin" value="Администратор" style="shape=umlActor;verticalLabelPosition=bottom;verticalAlign=top;html=1;outlineConnect=0;fillColor=#F3E8FF;strokeColor=#7C3AED;strokeWidth=2;fontSize=13;fontStyle=1;fontColor=#4C1D95;" vertex="1" parent="1">
          <mxGeometry x="1090" y="440" width="45" height="80" as="geometry" />
        </mxCell>
        
        <mxCell id="ext_payment" value="&amp;laquo;system&amp;raquo;&#10;Платёжный шлюз&#10;(Банк / СБП)" style="shape=rect;whiteSpace=wrap;html=1;fillColor=#F1F5F9;strokeColor=#64748B;strokeWidth=1.5;fontSize=11;fontStyle=1;fontColor=#334155;rounded=1;" vertex="1" parent="1">
          <mxGeometry x="1060" y="650" width="120" height="60" as="geometry" />
        </mxCell>

        <mxCell id="uc_apply" value="Подать заявление&#10;на заселение" style="ellipse;whiteSpace=wrap;html=1;fillColor=#FFFFFF;strokeColor=#2563EB;strokeWidth=1.5;fontSize=12;fontColor=#1E293B;" vertex="1" parent="1">
          <mxGeometry x="270" y="80" width="170" height="55" as="geometry" />
        </mxCell>
        <mxCell id="uc_benefit" value="Прикрепить документы&#10;о льготах" style="ellipse;whiteSpace=wrap;html=1;fillColor=#EFF6FF;strokeColor=#3B82F6;strokeWidth=1;fontSize=11;fontColor=#1E40AF;dashed=1;" vertex="1" parent="1">
          <mxGeometry x="490" y="75" width="160" height="50" as="geometry" />
        </mxCell>

        <mxCell id="uc_search_place" value="Подобрать свободное место&#10;(автопоиск)" style="ellipse;whiteSpace=wrap;html=1;fillColor=#EFF6FF;strokeColor=#2563EB;strokeWidth=2;fontSize=12;fontStyle=1;fontColor=#1E3A8A;" vertex="1" parent="1">
          <mxGeometry x="490" y="160" width="190" height="55" as="geometry" />
        </mxCell>

        <mxCell id="uc_distribute" value="Распределить место&#10;в общежитии" style="ellipse;whiteSpace=wrap;html=1;fillColor=#FFFFFF;strokeColor=#059669;strokeWidth=1.5;fontSize=12;fontColor=#1E293B;" vertex="1" parent="1">
          <mxGeometry x="270" y="240" width="170" height="55" as="geometry" />
        </mxCell>

        <mxCell id="uc_contract" value="Сформировать договор&#10;найма жилья" style="ellipse;whiteSpace=wrap;html=1;fillColor=#EFF6FF;strokeColor=#3B82F6;strokeWidth=1;fontSize=11;fontColor=#1E40AF;dashed=1;" vertex="1" parent="1">
          <mxGeometry x="490" y="270" width="170" height="50" as="geometry" />
        </mxCell>

        <mxCell id="uc_checkin" value="Оформить заселение&#10;и выдачу ключей" style="ellipse;whiteSpace=wrap;html=1;fillColor=#FFFFFF;strokeColor=#059669;strokeWidth=2;fontSize=12;fontStyle=1;fontColor=#064E3B;" vertex="1" parent="1">
          <mxGeometry x="270" y="340" width="170" height="60" as="geometry" />
        </mxCell>

        <mxCell id="uc_repair_req" value="Зарегистрировать заявку&#10;на ремонт" style="ellipse;whiteSpace=wrap;html=1;fillColor=#FFFFFF;strokeColor=#D97706;strokeWidth=1.5;fontSize=12;fontColor=#1E293B;" vertex="1" parent="1">
          <mxGeometry x="270" y="440" width="170" height="55" as="geometry" />
        </mxCell>

        <mxCell id="uc_repair_proc" value="Обработать и закрыть&#10;заявку на ремонт" style="ellipse;whiteSpace=wrap;html=1;fillColor=#FFFFFF;strokeColor=#059669;strokeWidth=1.5;fontSize=12;fontColor=#1E293B;" vertex="1" parent="1">
          <mxGeometry x="490" y="440" width="170" height="55" as="geometry" />
        </mxCell>

        <mxCell id="uc_pay" value="Оплатить проживание&#10;онлайн" style="ellipse;whiteSpace=wrap;html=1;fillColor=#FFFFFF;strokeColor=#2563EB;strokeWidth=1.5;fontSize=12;fontColor=#1E293B;" vertex="1" parent="1">
          <mxGeometry x="270" y="540" width="170" height="55" as="geometry" />
        </mxCell>

        <mxCell id="uc_debts" value="Контролировать задолженность&#10;жильцов" style="ellipse;whiteSpace=wrap;html=1;fillColor=#FFFFFF;strokeColor=#7C3AED;strokeWidth=1.5;fontSize=12;fontColor=#1E293B;" vertex="1" parent="1">
          <mxGeometry x="500" y="550" width="180" height="55" as="geometry" />
        </mxCell>

        <mxCell id="uc_checkout" value="Оформить выселение&#10;и обходной лист" style="ellipse;whiteSpace=wrap;html=1;fillColor=#FFFFFF;strokeColor=#059669;strokeWidth=1.5;fontSize=12;fontColor=#1E293B;" vertex="1" parent="1">
          <mxGeometry x="270" y="640" width="170" height="55" as="geometry" />
        </mxCell>

        <mxCell id="uc_damage" value="Зафиксировать ущерб&#10;и долги" style="ellipse;whiteSpace=wrap;html=1;fillColor=#FEF2F2;strokeColor=#DC2626;strokeWidth=1;fontSize=11;fontColor=#991B1B;dashed=1;" vertex="1" parent="1">
          <mxGeometry x="490" y="650" width="160" height="50" as="geometry" />
        </mxCell>

        <mxCell id="uc_rooms_manage" value="Вести учёт номерного фонда&#10;(корпуса, комнаты, места)" style="ellipse;whiteSpace=wrap;html=1;fillColor=#FFFFFF;strokeColor=#7C3AED;strokeWidth=1.5;fontSize=12;fontColor=#1E293B;" vertex="1" parent="1">
          <mxGeometry x="740" y="380" width="190" height="60" as="geometry" />
        </mxCell>

        <mxCell id="uc_reports" value="Формировать аналитические&#10;и статистические отчёты" style="ellipse;whiteSpace=wrap;html=1;fillColor=#FFFFFF;strokeColor=#7C3AED;strokeWidth=1.5;fontSize=12;fontColor=#1E293B;" vertex="1" parent="1">
          <mxGeometry x="740" y="500" width="190" height="60" as="geometry" />
        </mxCell>

        <mxCell id="uc_behavior" value="Контролировать соблюдение&#10;правил проживания" style="ellipse;whiteSpace=wrap;html=1;fillColor=#FFFFFF;strokeColor=#D97706;strokeWidth=1.5;fontSize=12;fontColor=#1E293B;" vertex="1" parent="1">
          <mxGeometry x="740" y="240" width="190" height="60" as="geometry" />
        </mxCell>

        <!-- Associations -->
        <mxCell id="a_st_apply" style="endArrow=none;strokeColor=#2563EB;strokeWidth=1.5;" edge="1" parent="1" source="act_student" target="uc_apply" />
        <mxCell id="a_st_repair" style="endArrow=none;strokeColor=#2563EB;strokeWidth=1.5;" edge="1" parent="1" source="act_student" target="uc_repair_req" />
        <mxCell id="a_st_pay" style="endArrow=none;strokeColor=#2563EB;strokeWidth=1.5;" edge="1" parent="1" source="act_student" target="uc_pay" />
        <mxCell id="a_st_cout" style="endArrow=none;strokeColor=#2563EB;strokeWidth=1.5;" edge="1" parent="1" source="act_student" target="uc_checkout" />

        <mxCell id="a_w_dist" style="endArrow=none;strokeColor=#059669;strokeWidth=1.5;" edge="1" parent="1" source="act_warden" target="uc_distribute" />
        <mxCell id="a_w_chin" style="endArrow=none;strokeColor=#059669;strokeWidth=1.5;" edge="1" parent="1" source="act_warden" target="uc_checkin" />
        <mxCell id="a_w_rproc" style="endArrow=none;strokeColor=#059669;strokeWidth=1.5;" edge="1" parent="1" source="act_warden" target="uc_repair_proc" />
        <mxCell id="a_w_cout" style="endArrow=none;strokeColor=#059669;strokeWidth=1.5;" edge="1" parent="1" source="act_warden" target="uc_checkout" />
        <mxCell id="a_w_rooms" style="endArrow=none;strokeColor=#059669;strokeWidth=1.5;" edge="1" parent="1" source="act_warden" target="uc_rooms_manage" />
        <mxCell id="a_w_debts" style="endArrow=none;strokeColor=#059669;strokeWidth=1.5;" edge="1" parent="1" source="act_warden" target="uc_debts" />

        <mxCell id="a_t_beh" style="endArrow=none;strokeColor=#D97706;strokeWidth=1.5;" edge="1" parent="1" source="act_tutor" target="uc_behavior" />
        <mxCell id="a_t_rep" style="endArrow=none;strokeColor=#D97706;strokeWidth=1.5;" edge="1" parent="1" source="act_tutor" target="uc_reports" />
        <mxCell id="a_t_repair" style="endArrow=none;strokeColor=#D97706;strokeWidth=1.5;" edge="1" parent="1" source="act_tutor" target="uc_repair_req" />

        <mxCell id="a_ad_rooms" style="endArrow=none;strokeColor=#7C3AED;strokeWidth=1.5;" edge="1" parent="1" source="act_admin" target="uc_rooms_manage" />
        <mxCell id="a_ad_debts" style="endArrow=none;strokeColor=#7C3AED;strokeWidth=1.5;" edge="1" parent="1" source="act_admin" target="uc_debts" />
        <mxCell id="a_ad_reports" style="endArrow=none;strokeColor=#7C3AED;strokeWidth=1.5;" edge="1" parent="1" source="act_admin" target="uc_reports" />

        <mxCell id="a_pay_ext" style="endArrow=none;strokeColor=#64748B;strokeWidth=1.5;dashed=1;" edge="1" parent="1" source="uc_pay" target="ext_payment" />

        <!-- Includes -->
        <mxCell id="inc_dist_search" value="&amp;laquo;include&amp;raquo;" style="dashed=1;endArrow=open;endFill=0;strokeColor=#2563EB;strokeWidth=1.5;fontSize=11;fontColor=#1E40AF;" edge="1" parent="1" source="uc_distribute" target="uc_search_place" />
        <mxCell id="inc_chin_dist" value="&amp;laquo;include&amp;raquo;" style="dashed=1;endArrow=open;endFill=0;strokeColor=#2563EB;strokeWidth=1.5;fontSize=11;fontColor=#1E40AF;" edge="1" parent="1" source="uc_checkin" target="uc_distribute" />
        <mxCell id="inc_chin_cont" value="&amp;laquo;include&amp;raquo;" style="dashed=1;endArrow=open;endFill=0;strokeColor=#2563EB;strokeWidth=1.5;fontSize=11;fontColor=#1E40AF;" edge="1" parent="1" source="uc_checkin" target="uc_contract" />

        <!-- Extends -->
        <mxCell id="ext_ben_apply" value="&amp;laquo;extend&amp;raquo;" style="dashed=1;endArrow=open;endFill=0;strokeColor=#DC2626;strokeWidth=1.5;fontSize=11;fontColor=#991B1B;" edge="1" parent="1" source="uc_benefit" target="uc_apply" />
        <mxCell id="ext_dam_cout" value="&amp;laquo;extend&amp;raquo;" style="dashed=1;endArrow=open;endFill=0;strokeColor=#DC2626;strokeWidth=1.5;fontSize=11;fontColor=#991B1B;" edge="1" parent="1" source="uc_damage" target="uc_checkout" />
    '''
    create_drawio_file(drawio_path, cells, 1250, 850)

# ==============================================================================
# 3. POLISHED ACTIVITY DIAGRAM
# ==============================================================================
def generate_activity_polished():
    drawio_path = "docs/model/activity.drawio"
    png_path = "docs/model/activity.png"
    
    html_doc = '''<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<style>
  body { font-family: 'Segoe UI', Arial, sans-serif; margin: 0; padding: 20px; background: #FFFFFF; color: #1E293B; }
  h2 { margin: 0 0 12px 0; color: #0F172A; font-size: 19px; text-align: center; font-weight: 700; }
  .canvas { position: relative; width: 1220px; height: 750px; border: 2px solid #CBD5E1; border-radius: 10px; background: #F8FAFC; margin: 0 auto; box-shadow: 0 4px 6px -1px rgba(0,0,0,0.05); }
  .lanes { display: flex; width: 100%; height: 100%; }
  .lane { flex: 1; border-right: 1.5px solid #CBD5E1; position: relative; }
  .lane:last-child { border-right: none; }
  .lane-head { height: 38px; display: flex; align-items: center; justify-content: center; font-weight: 700; font-size: 14px; border-bottom: 2px solid #CBD5E1; }
  .lh-st { background: #EFF6FF; color: #1E40AF; }
  .lh-wa { background: #ECFDF5; color: #065F46; }
  .lh-sy { background: #F5F3FF; color: #5B21B6; }

  .act { position: absolute; border-radius: 8px; background: white; padding: 8px 12px; font-size: 11px; text-align: center; box-shadow: 0 2px 4px rgba(0,0,0,0.06); width: 220px; box-sizing: border-box; }
  .act-st { border: 1.5px solid #3B82F6; color: #1E3A8A; }
  .act-wa { border: 1.5px solid #10B981; color: #064E3B; }
  .act-sy { border: 1.5px solid #8B5CF6; color: #4C1D95; }
  .act-core { background: #EDE9FE; border: 2px solid #7C3AED; font-weight: 700; }
  .act-err { background: #FEF2F2; border: 1.5px solid #EF4444; color: #991B1B; }

  .c-start { position: absolute; width: 24px; height: 24px; border-radius: 50%; background: #0F172A; }
  .c-end { position: absolute; width: 24px; height: 24px; border-radius: 50%; border: 3px solid #059669; background: #0F172A; }
  .c-end-err { position: absolute; width: 24px; height: 24px; border-radius: 50%; border: 3px solid #DC2626; background: #DC2626; }
  
  svg { position: absolute; top: 0; left: 0; width: 100%; height: 100%; pointer-events: none; }
  .edge-lbl { position: absolute; font-size: 11px; font-weight: 700; padding: 2px 6px; border-radius: 4px; }
  .lbl-yes { color: #047857; background: #D1FAE5; }
  .lbl-no { color: #B91C1C; background: #FEE2E2; }
</style>
</head>
<body>
  <h2>Диаграмма деятельности (Activity Diagram) — «Заселение с автопоиском и распределением места»</h2>
  <div class="canvas">
    <div class="lanes">
      <div class="lane" style="background: rgba(239, 246, 255, 0.4);">
        <div class="lane-head lh-st">👤 Студент</div>
      </div>
      <div class="lane" style="background: rgba(236, 253, 245, 0.4);">
        <div class="lane-head lh-wa">🏢 Комендант</div>
      </div>
      <div class="lane" style="background: rgba(245, 243, 255, 0.4);">
        <div class="lane-head lh-sy">⚙️ Система общежития</div>
      </div>
    </div>

    <!-- SVG for connections and clean decision diamonds -->
    <svg>
      <defs>
        <marker id="arr" viewBox="0 0 10 10" refX="8" refY="5" markerWidth="6" markerHeight="6" orient="auto-start-reverse">
          <path d="M 0 1 L 10 5 L 0 9 z" fill="#475569" />
        </marker>
        <marker id="arr-red" viewBox="0 0 10 10" refX="8" refY="5" markerWidth="6" markerHeight="6" orient="auto-start-reverse">
          <path d="M 0 1 L 10 5 L 0 9 z" fill="#DC2626" />
        </marker>
        <marker id="arr-green" viewBox="0 0 10 10" refX="8" refY="5" markerWidth="6" markerHeight="6" orient="auto-start-reverse">
          <path d="M 0 1 L 10 5 L 0 9 z" fill="#059669" />
        </marker>
      </defs>

      <!-- Start to Student act 1 -->
      <line x1="200" y1="70" x2="200" y2="90" stroke="#475569" stroke-width="1.8" marker-end="url(#arr)" />
      <!-- Student act 1 to System Validate -->
      <path d="M 310 115 L 900 115" fill="none" stroke="#475569" stroke-width="1.8" marker-end="url(#arr)" />
      <!-- System Validate to Warden Review -->
      <path d="M 900 140 L 610 180" fill="none" stroke="#475569" stroke-width="1.8" marker-end="url(#arr)" />
      <!-- Warden Review to Warden Trigger Search -->
      <line x1="610" y1="215" x2="610" y2="240" stroke="#475569" stroke-width="1.8" marker-end="url(#arr)" />
      <!-- Warden Trigger to System Search -->
      <line x1="720" y1="260" x2="900" y2="260" stroke="#475569" stroke-width="1.8" marker-end="url(#arr)" />
      
      <!-- System Search to Decision 1 -->
      <line x1="1010" y1="290" x2="1010" y2="320" stroke="#475569" stroke-width="1.8" marker-end="url(#arr)" />

      <!-- Decision 1 Rhombus (in SVG directly) -->
      <polygon points="1010,320 1090,355 1010,390 930,355" fill="#FEF3C7" stroke="#D97706" stroke-width="1.8" />
      <text x="1010" y="352" text-anchor="middle" font-size="11" font-weight="bold" fill="#78350F">Свободные места</text>
      <text x="1010" y="367" text-anchor="middle" font-size="11" font-weight="bold" fill="#78350F">найдены?</text>
      
      <!-- Decision 1 [Нет мест] -> Fail Queue -->
      <line x1="1010" y1="390" x2="1010" y2="430" stroke="#DC2626" stroke-width="1.8" marker-end="url(#arr-red)" />
      <!-- Fail Queue -> End Fail -->
      <line x1="1010" y1="480" x2="1010" y2="505" stroke="#DC2626" stroke-width="1.8" marker-end="url(#arr-red)" />

      <!-- Decision 1 [Место найдено] -> Student Decision -->
      <path d="M 930 355 L 310 355" fill="none" stroke="#059669" stroke-width="1.8" marker-end="url(#arr-green)" />

      <!-- Student Decision Rhombus (in SVG directly) -->
      <polygon points="200,325 285,355 200,385 115,355" fill="#FEF3C7" stroke="#D97706" stroke-width="1.8" />
      <text x="200" y="353" text-anchor="middle" font-size="11" font-weight="bold" fill="#78350F">Студент согласен</text>
      <text x="200" y="368" text-anchor="middle" font-size="11" font-weight="bold" fill="#78350F">с местом?</text>

      <!-- Student Decision [Согласен] -> Sign Contract -->
      <line x1="200" y1="385" x2="200" y2="515" stroke="#059669" stroke-width="1.8" marker-end="url(#arr-green)" />
      <!-- Student Decision [Отказ] -> System Queue -->
      <path d="M 285 370 Q 550 450 900 455" fill="none" stroke="#DC2626" stroke-width="1.8" marker-end="url(#arr-red)" />

      <!-- Student Sign -> System Lock and CheckIn -->
      <path d="M 310 540 L 900 540" fill="none" stroke="#475569" stroke-width="1.8" marker-end="url(#arr)" />
      <!-- System Lock -> Warden Issue Keys -->
      <path d="M 900 565 L 720 630" fill="none" stroke="#475569" stroke-width="1.8" marker-end="url(#arr)" />
      <!-- Warden Issue Keys -> Student Keys -->
      <line x1="500" y1="655" x2="310" y2="655" stroke="#475569" stroke-width="1.8" marker-end="url(#arr)" />
      <!-- Student Keys -> Final OK -->
      <line x1="200" y1="680" x2="200" y2="705" stroke="#059669" stroke-width="1.8" marker-end="url(#arr-green)" />
    </svg>

    <!-- HTML Steps -->
    <!-- Lane 1: Student -->
    <div class="c-start" style="top: 50px; left: 188px;"></div>
    <div class="act act-st" style="top: 90px; left: 90px;">1. Подать электронное заявление на заселение</div>
    <div class="act act-st" style="top: 515px; left: 90px;">5. Подписать договор найма онлайн в ЛК</div>
    <div class="act act-st" style="top: 630px; left: 90px;">7. Принять ключ и пропуск, заселиться в комнату</div>
    <div class="c-end" style="top: 705px; left: 188px;"></div>

    <!-- Lane 2: Warden -->
    <div class="act act-wa" style="top: 160px; left: 500px;">2. Проверить документы и льготный статус</div>
    <div class="act act-wa" style="top: 235px; left: 500px;">3. Инициировать автоподбор свободного места</div>
    <div class="act act-wa" style="top: 630px; left: 500px;">6. Сверить документы, выдать ключ и электронный пропуск</div>

    <!-- Lane 3: System -->
    <div class="act act-sy" style="top: 90px; left: 900px;">Валидация заявления и прикрепленных справок</div>
    <div class="act act-sy act-core" style="top: 235px; left: 900px;">🔍 Автопоиск свободных мест (фильтры: пол, курс, квоты)</div>
    <div class="act act-err" style="top: 430px; left: 900px;">Отклонить / Поместить в лист ожидания</div>
    <div class="c-end-err" style="top: 505px; left: 1000px;"></div>
    <div class="act act-sy" style="top: 515px; left: 900px;">🔒 Транзакционная фиксация (SELECT FOR UPDATE) и начисление платы</div>

    <!-- Branch Labels -->
    <div class="edge-lbl lbl-no" style="top: 395px; left: 1020px;">[Нет мест]</div>
    <div class="edge-lbl lbl-yes" style="top: 335px; left: 740px;">[Да, места найдены]</div>
    <div class="edge-lbl lbl-yes" style="top: 450px; left: 210px;">[Согласие]</div>
    <div class="edge-lbl lbl-no" style="top: 420px; left: 450px;">[Отказ студента]</div>
  </div>
</body>
</html>'''
    render_html_to_png(html_doc, png_path, 1300, 830)

# ==============================================================================
# 5. POLISHED COMPONENT DIAGRAM
# ==============================================================================
def generate_components_polished():
    png_path = "docs/architecture/components.png"
    
    html_doc = '''<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<style>
  body { font-family: 'Segoe UI', Arial, sans-serif; margin: 0; padding: 25px; background: #FFFFFF; color: #1E293B; }
  h2 { margin: 0 0 15px 0; color: #0F172A; font-size: 20px; text-align: center; font-weight: 700; }
  .canvas { position: relative; width: 1200px; height: 680px; border: 2px solid #CBD5E1; border-radius: 12px; background: #F8FAFC; margin: 0 auto; box-shadow: 0 4px 6px -1px rgba(0,0,0,0.05); }

  .monolith { position: absolute; top: 25px; left: 40px; width: 800px; height: 500px; border: 2.5px solid #2563EB; border-radius: 12px; background: #FFFFFF; }
  .mono-title { position: absolute; top: 12px; left: 18px; font-weight: 700; font-size: 15px; color: #1E40AF; display: flex; align-items: center; gap: 8px; }

  .comp { position: absolute; border-radius: 8px; background: white; padding: 8px 12px; box-shadow: 0 2px 4px rgba(0,0,0,0.06); font-size: 11px; text-align: center; }
  .c-box { border: 1.5px solid #3B82F6; color: #1E3A8A; }
  .c-core { background: #FFFBEB; border: 2px solid #D97706; color: #92400E; font-weight: 700; }
  .c-purple { background: #F5F3FF; border: 2px solid #7C3AED; color: #5B21B6; font-weight: 700; }
  .c-dal { width: 700px; background: #F1F5F9; border: 1.5px solid #475569; font-weight: 700; color: #1E293B; }

  .db { position: absolute; bottom: 25px; left: 330px; width: 220px; height: 90px; border: 2px solid #1D4ED8; border-radius: 12px; background: #EFF6FF; text-align: center; padding-top: 10px; box-sizing: border-box; }
  .ext { position: absolute; width: 230px; border: 1.5px dashed #64748B; border-radius: 8px; background: white; padding: 12px; text-align: center; font-size: 11px; }

  svg { position: absolute; top: 0; left: 0; width: 100%; height: 100%; pointer-events: none; }
  .iface { position: absolute; background: white; border: 1px solid #CBD5E1; padding: 3px 8px; border-radius: 4px; font-size: 10px; font-weight: 700; color: #475569; }
</style>
</head>
<body>
  <h2>Представление реализации: Диаграмма компонентов (UML Component Diagram)</h2>
  <div class="canvas">
    <div class="monolith">
      <div class="mono-title">📦 «component» Dormitory Management System (Модульный монолит)</div>

      <div class="comp c-box" style="top: 45px; left: 30px; width: 200px;">
        <b>Web UI & Controller</b><br>
        (REST API, HTML5/JS)
      </div>

      <div class="comp c-box" style="top: 45px; left: 270px; width: 220px;">
        <b>Security & Identity</b><br>
        (JWT, RBAC, Контур 152-ФЗ)
      </div>

      <div class="comp c-purple" style="top: 45px; left: 530px; width: 230px;">
        🔍 <b>Auto-Search Engine</b><br>
        (Алгоритм подбора свободных мест)
      </div>

      <div class="comp c-core" style="top: 155px; left: 40px; width: 330px;">
        🏠 <b>Tenancy & Check-In Core</b><br>
        (Заявления, Договоры, Выселение, Акты)
      </div>

      <div class="comp c-box" style="top: 155px; left: 420px; width: 340px;">
        🛠️ <b>Maintenance Module</b><br>
        (Журнал заявок на ремонт и дефектов)
      </div>

      <div class="comp c-box" style="top: 265px; left: 230px; width: 340px;">
        💳 <b>Billing & Debt Module</b><br>
        (Начисления, учёт оплат, реестр задолженностей)
      </div>

      <div class="comp c-dal" style="top: 380px; left: 50px;">
        🗄️ <b>Data Access Layer (ORM / Repository / Transaction Manager)</b><br>
        <span style="font-size:10px; font-weight:normal; color:#475569;">Строгая транзакционная изоляция (ACID) с блокировкой SELECT FOR UPDATE для исключения овербукинга</span>
      </div>
    </div>

    <!-- External DB -->
    <div class="db">
      <div style="font-size: 20px;">🛢️</div>
      <b>PostgreSQL Database</b><br>
      <span style="font-size: 10px; color: #1E40AF;">Схемы: auth, tenancy, rooms, billing</span>
    </div>

    <!-- External Services on right side -->
    <div class="ext" style="top: 160px; right: 40px;">
      <b>«external system»</b><br>
      🔔 <b>Сервис уведомлений</b><br>
      (Email / SMS шлюз отправки оповещений)
    </div>

    <div class="ext" style="top: 280px; right: 40px;">
      <b>«external system»</b><br>
      🏦 <b>Банковский платёжный шлюз</b><br>
      (СБП / Онлайн-эквайринг, вебхуки)
    </div>

    <!-- SVG lines -->
    <svg>
      <!-- Dal to DB -->
      <line x1="440" y1="525" x2="440" y2="575" stroke="#1D4ED8" stroke-width="2.5" />
      
      <!-- Core to Notify (Clean top curve avoiding other boxes) -->
      <path d="M 410 185 Q 600 135 930 190" fill="none" stroke="#64748B" stroke-width="1.8" stroke-dasharray="5,5" />
      
      <!-- Billing to Bank -->
      <line x1="610" y1="310" x2="930" y2="310" stroke="#64748B" stroke-width="1.8" stroke-dasharray="5,5" />
    </svg>

    <div class="iface" style="bottom: 125px; left: 450px;">TCP/IP (порт 5432)</div>
    <div class="iface" style="top: 140px; right: 290px;">HTTPS / REST</div>
    <div class="iface" style="top: 300px; right: 290px;">HTTPS / REST</div>
  </div>
</body>
</html>'''
    render_html_to_png(html_doc, png_path, 1300, 760)

# ==============================================================================
# 6. POLISHED DEPLOYMENT DIAGRAM
# ==============================================================================
def generate_deployment_polished():
    png_path = "docs/architecture/deployment.png"
    
    html_doc = '''<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<style>
  body { font-family: 'Segoe UI', Arial, sans-serif; margin: 0; padding: 25px; background: #FFFFFF; color: #1E293B; }
  h2 { margin: 0 0 15px 0; color: #0F172A; font-size: 20px; text-align: center; font-weight: 700; }
  .canvas { position: relative; width: 1250px; height: 680px; border: 2px solid #CBD5E1; border-radius: 12px; background: #F8FAFC; margin: 0 auto; box-shadow: 0 4px 6px -1px rgba(0,0,0,0.05); }

  .node { position: absolute; border-radius: 10px; background: white; box-shadow: 0 4px 6px -1px rgba(0,0,0,0.08); }
  .node-head { padding: 8px 12px; font-weight: 700; font-size: 13px; border-radius: 8px 8px 0 0; display: flex; align-items: center; justify-content: space-between; }
  .node-body { padding: 12px; font-size: 11px; }

  .n-client { border: 2px solid #3B82F6; }
  .n-client .node-head { background: #EFF6FF; color: #1E40AF; border-bottom: 1.5px solid #BFDBFE; }

  .n-app { border: 2px solid #10B981; }
  .n-app .node-head { background: #ECFDF5; color: #065F46; border-bottom: 1.5px solid #A7F3D0; }

  .n-db { border: 2px solid #D97706; }
  .n-db .node-head { background: #FEF3C7; color: #92400E; border-bottom: 1.5px solid #FDE68A; }

  .n-ext { border: 1.5px dashed #64748B; }
  .n-ext .node-head { background: #F1F5F9; color: #334155; border-bottom: 1.5px solid #CBD5E1; }

  .art { border: 1px solid #CBD5E1; border-radius: 6px; background: #F8FAFC; padding: 8px 10px; margin-bottom: 8px; font-size: 11px; }
  .art:last-child { margin-bottom: 0; }
  .art-title { font-weight: 700; margin-bottom: 4px; display: flex; align-items: center; gap: 6px; }

  svg { position: absolute; top: 0; left: 0; width: 100%; height: 100%; pointer-events: none; }
  .comm { position: absolute; background: white; border: 1.5px solid #94A3B8; padding: 4px 8px; border-radius: 6px; font-size: 10px; font-weight: 700; color: #334155; text-align: center; width: 135px; box-sizing: border-box; }
</style>
</head>
<body>
  <h2>Представление развёртывания: Диаграмма развёртывания (UML Deployment Diagram)</h2>
  <div class="canvas">
    <!-- SVG Connections -->
    <svg>
      <!-- Client to App Server -->
      <line x1="290" y1="180" x2="470" y2="180" stroke="#2563EB" stroke-width="2.5" />

      <!-- App Server to DB Server -->
      <line x1="630" y1="365" x2="630" y2="440" stroke="#D97706" stroke-width="2.5" />

      <!-- App Server to External -->
      <line x1="790" y1="180" x2="970" y2="180" stroke="#64748B" stroke-width="2" stroke-dasharray="5,5" />
    </svg>

    <!-- Communication Labels placed in gaps -->
    <div class="comm" style="top: 150px; left: 300px; border-color: #2563EB;">
      <b>HTTPS (TLS 1.3, порт 443)</b><br>
      Шифрованный интернет / локальная сеть
    </div>

    <div class="comm" style="top: 380px; left: 645px; border-color: #D97706;">
      <b>TCP (порт 5432, TLS)</b><br>
      Изолированная VLAN (без выхода наружу)
    </div>

    <div class="comm" style="top: 150px; left: 805px; border-color: #64748B;">
      <b>HTTPS / mTLS REST</b><br>
      Банковский платёжный API
    </div>

    <!-- Node 1: Client Device -->
    <div class="node n-client" style="top: 70px; left: 30px; width: 260px;">
      <div class="node-head">
        <span>💻 «device» Клиент</span>
        <span style="font-size: 11px;">ПК / Смартфон</span>
      </div>
      <div class="node-body">
        <div class="art">
          <div class="art-title">🌐 «artifact» Веб-браузер (Web Browser)</div>
          Клиентский интерфейс системы управления общежитием (SPA / HTML5 / CSS3 / JavaScript).
        </div>
        <div style="font-size: 10px; color: #64748B; margin-top: 6px;">
          Роли: Студент, Комендант, Воспитатель, Администратор.
        </div>
      </div>
    </div>

    <!-- Node 2: App Server -->
    <div class="node n-app" style="top: 45px; left: 470px; width: 320px;">
      <div class="node-head">
        <span>🖥️ «device / server» Сервер приложений</span>
        <span style="font-size: 11px;">Linux Ubuntu</span>
      </div>
      <div class="node-body">
        <div class="art">
          <div class="art-title">🛡️ «artifact» Nginx Reverse Proxy</div>
          SSL-терминация (TLS 1.3), балансировка нагрузки, защита от DDoS, статический контент.
        </div>
        <div class="art">
          <div class="art-title">⚙️ «artifact» dormitory-app.jar</div>
          Модульный монолит: Web API, движок автопоиска мест, ядро заселения, биллинг, проверка RBAC.
        </div>
      </div>
    </div>

    <!-- Node 3: DB Server -->
    <div class="node n-db" style="top: 440px; left: 470px; width: 320px;">
      <div class="node-head">
        <span>🛢️ «device / server» Сервер БД</span>
        <span style="font-size: 10px; color: #92400E; font-weight: bold;">Контур ПДн (152-ФЗ)</span>
      </div>
      <div class="node-body">
        <div class="art">
          <div class="art-title">🔒 «artifact» PostgreSQL 16 DBMS</div>
          Реляционная СУБД: зашифрованные разделы с персональными данными (ФИО, паспорт, телефон), договоры, счета.<br>
          <b>Изоляция:</b> доступ разрешён строго с IP сервера приложений через файрвол UFW.
        </div>
      </div>
    </div>

    <!-- Node 4: External Gateway -->
    <div class="node n-ext" style="top: 90px; right: 30px; width: 250px;">
      <div class="node-head">
        <span>🏦 «external node» Платёжный шлюз</span>
      </div>
      <div class="node-body">
        <div class="art">
          <div class="art-title">💳 Банк-эквайер / СБП</div>
          Внешняя банковская инфраструктура обработки онлайн-платежей с возвратом вебхуков подтверждения.
        </div>
      </div>
    </div>

  </div>
</body>
</html>'''
    render_html_to_png(html_doc, png_path, 1350, 750)

if __name__ == "__main__":
    generate_activity_polished()
    generate_components_polished()
    generate_deployment_polished()
    print("Polish complete!")
