/**
 * Dormitory Management System (Общежитие-Смарт)
 * Interactive Application Logic & Mock State Engine
 * Student: Vitenik P.L.
 */

// =============================================================================
// 1. APPLICATION DATA STORE & MOCK DATABASE
// =============================================================================
const DB = {
  currentUser: {
    name: "Витеник П.Л.",
    role: "student",
    studentCard: "СТ-2024-0412",
    gender: "male",
    faculty: "Информационные технологии",
    course: 3,
    room: "304",
    bed: 2,
    hasBenefit: false,
    balance: 0
  },

  rooms: [
    // 3rd Floor (Male block)
    { id: 301, number: "301", floor: 3, gender: "male", type: "2-bed", capacity: 2, beds: [{ num: 1, status: "occupied", student: "Иванов А.С." }, { num: 2, status: "occupied", student: "Петров Д.М." }] },
    { id: 302, number: "302", floor: 3, gender: "male", type: "3-bed", capacity: 3, beds: [{ num: 1, status: "occupied", student: "Сидоров К.В." }, { num: 2, status: "free", student: null }, { num: 3, status: "free", student: null }] },
    { id: 303, number: "303", floor: 3, gender: "male", type: "2-bed", capacity: 2, beds: [{ num: 1, status: "occupied", student: "Смирнов М.И." }, { num: 2, status: "reserved", student: "Козлов Е.А." }] },
    { id: 304, number: "304", floor: 3, gender: "male", type: "2-bed", capacity: 2, beds: [{ num: 1, status: "occupied", student: "Васильев О.П." }, { num: 2, status: "occupied", student: "Витеник П.Л." }] },
    { id: 305, number: "305", floor: 3, gender: "male", type: "3-bed", capacity: 3, beds: [{ num: 1, status: "free", student: null }, { num: 2, status: "free", student: null }, { num: 3, status: "occupied", student: "Николаев Т.С." }] },
    
    // 2nd Floor (Female block)
    { id: 201, number: "201", floor: 2, gender: "female", type: "2-bed", capacity: 2, beds: [{ num: 1, status: "occupied", student: "Морозова А.В." }, { num: 2, status: "free", student: null }] },
    { id: 202, number: "202", floor: 2, gender: "female", type: "2-bed", capacity: 2, beds: [{ num: 1, status: "occupied", student: "Кузнецова Е.Н." }, { num: 2, status: "occupied", student: "Павлова С.И." }] },
    { id: 203, number: "203", floor: 2, gender: "female", type: "3-bed", capacity: 3, beds: [{ num: 1, status: "free", student: null }, { num: 2, status: "free", student: null }, { num: 3, status: "free", student: null }] },

    // 4th Floor
    { id: 401, number: "401", floor: 4, gender: "male", type: "2-bed", capacity: 2, beds: [{ num: 1, status: "occupied", student: "Зайцев Г.Ю." }, { num: 2, status: "free", student: null }] },
    { id: 402, number: "402", floor: 4, gender: "male", type: "2-bed", capacity: 2, beds: [{ num: 1, status: "free", student: null }, { num: 2, status: "free", student: null }] }
  ],

  applications: [
    { id: "APP-101", student: "Витеник П.Л.", gender: "male", course: 3, faculty: "Информационные технологии", roomPref: "2-bed", benefit: "none", status: "Заселен", date: "01.09.2026", allocatedRoom: "304", allocatedBed: 2 },
    { id: "APP-102", student: "Григорьев М.К.", gender: "male", course: 3, faculty: "Информационные технологии", roomPref: "2-bed", benefit: "orphan", status: "Ожидает подбора", date: "08.10.2026", allocatedRoom: null, allocatedBed: null },
    { id: "APP-103", student: "Сергеева А.Д.", gender: "female", course: 1, faculty: "Экономика", roomPref: "2-bed", benefit: "none", status: "Ожидает подбора", date: "09.10.2026", allocatedRoom: null, allocatedBed: null },
    { id: "APP-104", student: "Дмитриев С.В.", gender: "male", course: 2, faculty: "Машиностроение", roomPref: "3-bed", benefit: "target", status: "Ожидает подбора", date: "09.10.2026", allocatedRoom: null, allocatedBed: null }
  ],

  repairs: [
    { id: "REQ-401", room: "304", category: "сантехника", desc: "Подкапывает кран умывальника в санузле", status: "Мастер назначен", date: "08.10.2026", student: "Витеник П.Л." },
    { id: "REQ-398", room: "301", category: "электрика", desc: "Замена светодиодной лампы", status: "Выполнено", date: "03.10.2026", student: "Иванов А.С." }
  ],

  debtors: [
    { student: "Сидоров К.В.", room: "302/1", contract: "ДН-2025/112", period: "2 месяца", amount: 3700 },
    { student: "Николаев Т.С.", room: "305/3", contract: "ДН-2025/084", period: "1 месяц", amount: 1850 },
    { student: "Морозова А.В.", room: "201/1", contract: "ДН-2026/014", period: "1 месяц", amount: 1850 }
  ]
};

// =============================================================================
// 2. DOM INITIALIZATION & EVENT LISTENERS
// =============================================================================
document.addEventListener("DOMContentLoaded", () => {
  initRoleSwitcher();
  renderStudentView();
  renderWardenView();
  renderTutorView();
  renderAdminView();
  initModals();
  initAutoSearchEngine();
  initForms();
});

// Toast notification helper
function showToast(message, type = "success") {
  const container = document.getElementById("toast-container");
  const toast = document.createElement("div");
  toast.className = `toast toast-${type}`;
  toast.innerHTML = `<span>${type === "success" ? "✅" : "ℹ️"}</span> <div>${message}</div>`;
  container.appendChild(toast);
  setTimeout(() => {
    toast.style.opacity = "0";
    toast.style.transform = "translateX(100%)";
    setTimeout(() => toast.remove(), 300);
  }, 3500);
}

// =============================================================================
// 3. ROLE SWITCHING CONTROLLER
// =============================================================================
function initRoleSwitcher() {
  const roleButtons = document.querySelectorAll(".role-btn");
  const roleViews = document.querySelectorAll(".role-view");
  const avatar = document.getElementById("current-user-avatar");
  const userName = document.getElementById("current-user-name");
  const userBadge = document.getElementById("current-user-badge");

  roleButtons.forEach(btn => {
    btn.addEventListener("click", () => {
      const selectedRole = btn.dataset.role;

      // Update button states
      roleButtons.forEach(b => b.classList.remove("active"));
      btn.classList.add("active");

      // Update views
      roleViews.forEach(v => v.classList.remove("active"));
      const targetView = document.getElementById(`view-${selectedRole}`);
      if (targetView) targetView.classList.add("active");

      // Update Header Profile Info
      switch (selectedRole) {
        case "student":
          avatar.innerText = "ВП";
          avatar.style.background = "linear-gradient(135deg, #059669, #10b981)";
          userName.innerText = "Витеник П.Л.";
          userBadge.innerText = "Студент (комн. 304/2)";
          break;
        case "warden":
          avatar.innerText = "КМ";
          avatar.style.background = "linear-gradient(135deg, #2563eb, #3b82f6)";
          userName.innerText = "Соколова Н.В.";
          userBadge.innerText = "Комендант Корпуса №1";
          break;
        case "tutor":
          avatar.innerText = "ВП";
          avatar.style.background = "linear-gradient(135deg, #d97706, #f59e0b)";
          userName.innerText = "Кузнецов И.А.";
          userBadge.innerText = "Старший воспитатель";
          break;
        case "admin":
          avatar.innerText = "АД";
          avatar.style.background = "linear-gradient(135deg, #7c3aed, #8b5cf6)";
          userName.innerText = "Системный администратор";
          userBadge.innerText = "Администратор Студгородка";
          break;
      }

      showToast(`Переключен профиль: ${userName.innerText}`, "info");
    });
  });
}

// =============================================================================
// 4. STUDENT VIEW RENDERER
// =============================================================================
function renderStudentView() {
  const table = document.getElementById("student-apps-table");
  table.innerHTML = "";

  DB.applications.forEach(app => {
    const tr = document.createElement("tr");
    tr.innerHTML = `
      <td><strong>${app.id}</strong></td>
      <td>${app.date}</td>
      <td>${app.roomPref === "2-bed" ? "2-местная" : "3-местная"}</td>
      <td><span class="badge ${app.status === "Заселен" ? "badge-success" : "badge-warning"}">${app.status}</span></td>
      <td>
        <button class="btn btn-sm btn-outline btn-view-contract" data-id="${app.id}">
          Просмотр договора
        </button>
      </td>
    `;
    table.appendChild(tr);
  });

  // Render Repairs list
  const repairsContainer = document.getElementById("student-repairs-list");
  repairsContainer.innerHTML = "";
  DB.repairs.forEach(rep => {
    const item = document.createElement("div");
    item.className = "repair-item";
    item.innerHTML = `
      <div class="repair-icon">${rep.category === "сантехника" ? "🚰" : "💡"}</div>
      <div style="flex:1;">
        <div class="repair-desc">${rep.desc}</div>
        <div class="repair-date">Комната ${rep.room} • ${rep.date} • <span class="badge badge-info">${rep.status}</span></div>
      </div>
    `;
    repairsContainer.appendChild(item);
  });

  // Attach contract preview buttons
  document.querySelectorAll(".btn-view-contract").forEach(btn => {
    btn.addEventListener("click", () => {
      openContractModal();
    });
  });
}

// =============================================================================
// 5. WARDEN VIEW & ROOM MAP RENDERER
// =============================================================================
function renderWardenView() {
  const grid = document.getElementById("rooms-visual-grid");
  const floorFilter = document.getElementById("floor-filter").value;
  const statusFilter = document.getElementById("status-filter").value;

  grid.innerHTML = "";

  const filteredRooms = DB.rooms.filter(room => {
    if (floorFilter !== "all" && room.floor !== parseInt(floorFilter)) return false;
    if (statusFilter === "free" && !room.beds.some(b => b.status === "free")) return false;
    if (statusFilter === "occupied" && room.beds.every(b => b.status === "free")) return false;
    return true;
  });

  filteredRooms.forEach(room => {
    const card = document.createElement("div");
    card.className = "room-card";
    
    let bedsHtml = "";
    room.beds.forEach(bed => {
      bedsHtml += `
        <div class="bed-badge ${bed.status}" title="Место №${bed.num} (${bed.student || 'Свободно'})" onclick="onBedClick('${room.number}', ${bed.num}, '${bed.status}')">
          №${bed.num} ${bed.status === 'free' ? 'Свободно' : (bed.status === 'reserved' ? 'Бронь' : 'Занято')}
        </div>
      `;
    });

    card.innerHTML = `
      <div class="room-card-head">
        <span class="room-num">🚪 Комната ${room.number}</span>
        <span class="room-type-tag">${room.gender === 'male' ? '🚹 Муж' : '🚺 Жен'} • ${room.type}</span>
      </div>
      <div class="beds-row">
        ${bedsHtml}
      </div>
    `;
    grid.appendChild(card);
  });

  // Render incoming applications table
  const appsTable = document.getElementById("warden-apps-table");
  appsTable.innerHTML = "";

  const pendingApps = DB.applications.filter(a => a.status === "Ожидает подбора");
  document.getElementById("badge-incoming-count").innerText = `${pendingApps.length} требуют решения`;

  pendingApps.forEach(app => {
    const tr = document.createElement("tr");
    tr.innerHTML = `
      <td><strong>${app.id}</strong></td>
      <td>${app.student}</td>
      <td>${app.gender === 'male' ? 'Муж' : 'Жен'} / ${app.course} курс</td>
      <td>${app.faculty}</td>
      <td>${app.benefit !== 'none' ? '<span class="badge badge-purple">Льгота</span>' : 'Обычный'}</td>
      <td id="alloc-target-${app.id}">${app.allocatedRoom ? `Комн. ${app.allocatedRoom}/${app.allocatedBed}` : '<em>Не назначено</em>'}</td>
      <td>
        <button class="btn btn-sm btn-primary btn-auto-pick" data-id="${app.id}">
          ⚡ Автоподбор
        </button>
        <button class="btn btn-sm btn-success btn-confirm-checkin" data-id="${app.id}" style="${app.allocatedRoom ? '' : 'display:none;'}">
          Заселить
        </button>
      </td>
    `;
    appsTable.appendChild(tr);
  });

  // Attach auto-pick buttons
  document.querySelectorAll(".btn-auto-pick").forEach(btn => {
    btn.addEventListener("click", () => {
      const appId = btn.dataset.id;
      const app = DB.applications.find(a => a.id === appId);
      if (app) {
        runAutoSearchForStudent(app);
      }
    });
  });

  // Attach check-in confirmation buttons
  document.querySelectorAll(".btn-confirm-checkin").forEach(btn => {
    btn.addEventListener("click", () => {
      const appId = btn.dataset.id;
      confirmCheckIn(appId);
    });
  });
}

// Bed click handler
window.onBedClick = function(roomNumber, bedNum, status) {
  if (status === "free") {
    showToast(`Койко-место №${bedNum} в комнате ${roomNumber} свободно и готово к заселению!`, "info");
  } else {
    showToast(`Койко-место №${bedNum} в комнате ${roomNumber} имеет статус: ${status}`, "info");
  }
};

// =============================================================================
// 6. AUTO-SEARCH ENGINE LOGIC (Ключевая возможность задания)
// =============================================================================
function initAutoSearchEngine() {
  document.getElementById("floor-filter").addEventListener("change", renderWardenView);
  document.getElementById("status-filter").addEventListener("change", renderWardenView);

  const btnOpenSearch = document.getElementById("btn-run-auto-search");
  btnOpenSearch.addEventListener("click", () => {
    document.getElementById("modal-auto-search").classList.add("show");
  });

  const btnExecSearch = document.getElementById("btn-execute-search");
  btnExecSearch.addEventListener("click", () => {
    const gender = document.getElementById("search-gender").value;
    const course = document.getElementById("search-course").value;
    const roomType = document.getElementById("search-room-type").value;

    const results = findAvailableBeds(gender, roomType);
    const box = document.getElementById("search-results-box");
    const countSpan = document.getElementById("results-count");
    const list = document.getElementById("results-list-container");

    countSpan.innerText = results.length;
    list.innerHTML = "";

    if (results.length === 0) {
      list.innerHTML = "<p style='color:var(--danger);'>Подходящих свободных мест по данным критериям не найдено.</p>";
    } else {
      results.forEach(res => {
        const item = document.createElement("div");
        item.className = "result-card";
        item.innerHTML = `
          <div class="result-info">
            <strong>Комната ${res.roomNumber} (Этаж ${res.floor}) — Место №${res.bedNumber}</strong>
            <p>${res.type === '2-bed' ? '2-местная' : '3-местная'} • ${res.gender === 'male' ? 'Мужской сектор' : 'Женский сектор'}</p>
          </div>
          <button class="btn btn-sm btn-primary" onclick="allocateBedQuick('${res.roomNumber}', ${res.bedNumber})">
            Выбрать
          </button>
        `;
        list.appendChild(item);
      });
    }

    box.style.display = "block";
  });
}

function findAvailableBeds(gender, roomType = "any") {
  const matches = [];
  DB.rooms.forEach(room => {
    if (room.gender === gender) {
      if (roomType === "any" || room.type === roomType) {
        room.beds.forEach(bed => {
          if (bed.status === "free") {
            matches.push({
              roomId: room.id,
              roomNumber: room.number,
              floor: room.floor,
              type: room.type,
              gender: room.gender,
              bedNumber: bed.num
            });
          }
        });
      }
    }
  });
  return matches;
}

function runAutoSearchForStudent(app) {
  const matches = findAvailableBeds(app.gender, app.roomPref);
  if (matches.length > 0) {
    const pick = matches[0];
    app.allocatedRoom = pick.roomNumber;
    app.allocatedBed = pick.bedNumber;

    // Reserve bed
    const room = DB.rooms.find(r => r.number === pick.roomNumber);
    const bed = room.beds.find(b => b.num === pick.bedNumber);
    bed.status = "reserved";
    bed.student = app.student;

    showToast(`⚡ Автопоиск: Студенту ${app.student} подобрана комната ${pick.roomNumber}, место №${pick.bedNumber}!`, "success");
    renderWardenView();
  } else {
    showToast(`Мест для студента ${app.student} не найдено! Заявка направлена в лист ожидания.`, "danger");
  }
}

window.allocateBedQuick = function(roomNumber, bedNumber) {
  showToast(`Место №${bedNumber} в комнате ${roomNumber} успешно выбрано!`, "success");
  document.getElementById("modal-auto-search").classList.remove("show");
  renderWardenView();
};

function confirmCheckIn(appId) {
  const app = DB.applications.find(a => a.id === appId);
  if (!app) return;

  app.status = "Заселен";
  const room = DB.rooms.find(r => r.number === app.allocatedRoom);
  if (room) {
    const bed = room.beds.find(b => b.num === app.allocatedBed);
    if (bed) {
      bed.status = "occupied";
      bed.student = app.student;
    }
  }

  showToast(`🎉 Заселение завершено! Договор сформирован, ключи и пропуск выданы студенту ${app.student}.`, "success");
  renderWardenView();
  renderStudentView();
}

// =============================================================================
// 7. TUTOR & ADMIN VIEWS
// =============================================================================
function renderTutorView() {
  const tbody = document.getElementById("tutor-students-table");
  tbody.innerHTML = "";

  const students = [
    { fio: "Витеник П.Л.", room: "304", group: "ИТ-31 / 3 курс", phone: "+7 (999) 123-45-67", status: "Без замечаний" },
    { fio: "Иванов А.С.", room: "301", group: "ИТ-31 / 3 курс", phone: "+7 (999) 234-56-78", status: "Без замечаний" },
    { fio: "Сидоров К.В.", room: "302", group: "МШ-22 / 2 курс", phone: "+7 (999) 345-67-89", status: "Предупреждение (шум)" },
    { fio: "Морозова А.В.", room: "201", group: "ЭК-11 / 1 курс", phone: "+7 (999) 456-78-90", status: "Без замечаний" }
  ];

  students.forEach(st => {
    const tr = document.createElement("tr");
    tr.innerHTML = `
      <td><strong>${st.fio}</strong></td>
      <td>Комната ${st.room}</td>
      <td>${st.group}</td>
      <td>${st.phone}</td>
      <td><span class="badge ${st.status.includes('Предупреждение') ? 'badge-warning' : 'badge-success'}">${st.status}</span></td>
    `;
    tbody.appendChild(tr);
  });

  const benefitsList = document.getElementById("tutor-benefits-list");
  benefitsList.innerHTML = `
    <div style="border: 1px solid var(--border); border-radius: var(--radius-md); padding: 12px; margin-bottom: 10px;">
      <div style="display:flex; justify-content:space-between; align-items:center;">
        <strong>Григорьев М.К. (Сирота / Опека)</strong>
        <span class="badge badge-purple">100% квота</span>
      </div>
      <p style="font-size:12px; color:var(--text-muted); margin: 6px 0;">Документы проверены и заверены отделом опеки.</p>
      <button class="btn btn-sm btn-success" onclick="showToast('Льгота согласована воспитателем!')">Согласовать заселение</button>
    </div>
  `;
}

function renderAdminView() {
  const tbody = document.getElementById("admin-debtors-table");
  tbody.innerHTML = "";

  DB.debtors.forEach(d => {
    const tr = document.createElement("tr");
    tr.innerHTML = `
      <td><strong>${d.student}</strong></td>
      <td>Комната ${d.room}</td>
      <td>${d.contract}</td>
      <td><span class="badge badge-danger">${d.period}</span></td>
      <td><strong>${d.amount} ₽</strong></td>
      <td>
        <button class="btn btn-sm btn-outline" onclick="showToast('Уведомление и СМС отправлены должнику!')">Напомнить</button>
      </td>
    `;
    tbody.appendChild(tr);
  });

  document.getElementById("btn-export-excel").addEventListener("click", () => {
    showToast("📊 Реестр должников успешно выгружен в формате Excel/CSV!", "info");
  });
}

// =============================================================================
// 8. MODALS & FORMS
// =============================================================================
function initModals() {
  document.querySelectorAll("[data-close]").forEach(el => {
    el.addEventListener("click", () => {
      const modalId = el.dataset.close;
      document.getElementById(modalId).classList.remove("show");
    });
  });

  document.getElementById("btn-open-application-modal").addEventListener("click", () => {
    document.getElementById("modal-application").classList.add("show");
  });

  document.getElementById("btn-open-repair-modal").addEventListener("click", () => {
    document.getElementById("modal-repair").classList.add("show");
  });

  document.getElementById("btn-add-repair-quick").addEventListener("click", () => {
    document.getElementById("modal-repair").classList.add("show");
  });

  document.getElementById("btn-quick-pay").addEventListener("click", () => {
    document.getElementById("modal-payment").classList.add("show");
  });

  document.getElementById("btn-confirm-payment-action").addEventListener("click", () => {
    showToast("💳 Платёж на сумму 1 850 ₽ успешно проведён через СБП! Чек отправлен на почту.", "success");
    document.getElementById("modal-payment").classList.remove("show");
  });

  document.getElementById("btn-request-checkout").addEventListener("click", () => {
    showToast("Заявление на выселение принято. Коменданту передан электронный обходной лист.", "info");
  });
}

function openContractModal() {
  const modal = document.getElementById("modal-contract");
  const preview = document.getElementById("contract-doc-text");
  preview.innerHTML = `
    <h4>ДОГОВОР НАЙМА ЖИЛОГО ПОМЕЩЕНИЯ В СТУДЕНЧЕСКОМ ОБЩЕЖИТИИ № ДН-2026/089</h4>
    <p><strong>г. Брянск</strong> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <strong>«01» сентября 2026 г.</strong></p>
    <br>
    <p>Политехнический колледж БГТУ, именуемый в дальнейшем <em>«Наймодатель»</em>, в лице коменданта Соколовой Н.В., с одной стороны, и студент <strong>Витеник П.Л.</strong>, именуемый в дальнейшем <em>«Наниматель»</em>, заключили настоящий Договор о нижеследующем:</p>
    <br>
    <p><strong>1. Предмет договора:</strong> Наймодатель предоставляет Нанимателю койко-место №2 в жилой комнате №304 Корпуса №1 общежития колледжа для временного проживания на период обучения.</p>
    <p><strong>2. Плата за жилое помещение:</strong> Размер ежемесячной платы за проживание и коммунальные услуги составляет <strong>1 850 (одна тысяча восемьсот пятьдесят) рублей</strong>. Оплата производится не позднее 10 числа каждого месяца.</p>
    <p><strong>3. Срок действия:</strong> Договор действует до 30 июня 2027 г.</p>
    <p><strong>4. Особые условия:</strong> При выселении Наниматель обязуется сдать помещение, инвентарь и ключи в надлежащем состоянии по обходному листу.</p>
  `;
  modal.classList.add("show");

  document.getElementById("btn-sign-contract-action").onclick = () => {
    showToast("✍️ Договор найма № ДН-2026/089 успешно подписан простой электронной подписью!", "success");
    modal.classList.remove("show");
  };
}

function initForms() {
  document.getElementById("form-new-application").addEventListener("submit", (e) => {
    e.preventDefault();
    const fio = document.getElementById("app-fio").value;
    const faculty = document.getElementById("app-faculty").value;
    const course = parseInt(document.getElementById("app-course").value);
    const gender = document.getElementById("app-gender").value;
    const roomPref = document.getElementById("app-room-pref").value;
    const benefit = document.getElementById("app-benefit").value;

    const newApp = {
      id: `APP-${100 + DB.applications.length + 1}`,
      student: fio,
      gender: gender,
      course: course,
      faculty: faculty,
      roomPref: roomPref,
      benefit: benefit,
      status: "Ожидает подбора",
      date: "09.10.2026",
      allocatedRoom: null,
      allocatedBed: null
    };

    DB.applications.unshift(newApp);
    showToast(`Заявление ${newApp.id} успешно подано! Комендант запустит автоподбор.`, "success");
    document.getElementById("modal-application").classList.remove("show");
    renderStudentView();
    renderWardenView();
  });

  document.getElementById("form-new-repair").addEventListener("submit", (e) => {
    e.preventDefault();
    const room = document.getElementById("rep-room").value;
    const cat = document.getElementById("rep-category").value;
    const desc = document.getElementById("rep-desc").value;

    const newRep = {
      id: `REQ-${400 + DB.repairs.length + 1}`,
      room: room,
      category: cat,
      desc: desc,
      status: "Принята в работу",
      date: "09.10.2026",
      student: DB.currentUser.name
    };

    DB.repairs.unshift(newRep);
    showToast(`Заявка на ремонт ${newRep.id} зарегистрирована! Мастер прибудет в ближайшее время.`, "success");
    document.getElementById("modal-repair").classList.remove("show");
    renderStudentView();
  });
}
