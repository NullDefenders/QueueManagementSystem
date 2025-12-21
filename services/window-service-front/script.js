class WindowService {
  constructor() {
    this.apiUrl = "https://localhost:7155/api/Window";
    this.history = JSON.parse(localStorage.getItem("windowHistory")) || [];
    this.init();
  }

  init() {
    // Инициализация обработчиков событий
    document
      .getElementById("sendButton")
      .addEventListener("click", () => this.sendWindowStatus());

    // Обновление статуса при изменении радиокнопок
    document.querySelectorAll('input[name="status"]').forEach((radio) => {
      radio.addEventListener("change", (e) =>
        this.updateStatusDisplay(e.target.value)
      );
    });

    // Загрузка истории
    this.loadHistory();

    // Инициализация отображения статуса
    this.updateStatusDisplay("free");
  }

  updateStatusDisplay(status) {
    const statusDisplay = document.getElementById("currentStatus");
    const statusText = status === "free" ? "Свободно" : "Занято";
    const statusClass = status === "free" ? "free" : "busy";
    const icon = status === "free" ? "fa-check-circle" : "fa-user-clock";

    statusDisplay.innerHTML = `
            <span class="status-badge ${statusClass}">
                <i class="fas ${icon}"></i> ${statusText}
            </span>
        `;
  }

  async sendWindowStatus() {
    const button = document.getElementById("sendButton");
    const originalText = button.innerHTML;

    try {
      // Блокируем кнопку на время отправки
      button.disabled = true;
      button.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Отправка...';

      // Получаем данные из формы
      const windowData = this.getWindowData();

      // Отправляем данные на сервер
      const response = await fetch(this.apiUrl, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(windowData),
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      const result = await response.json();

      // Обновляем интерфейс
      this.showResult("success", "✅ Статус успешно отправлен в RabbitMQ");

      // Добавляем в историю
      this.addToHistory({
        ...windowData,
        success: true,
      });
    } catch (error) {
      console.error("Error sending window status:", error);
      this.showResult("error", "❌ Ошибка при отправке: " + error.message);

      // Добавляем ошибку в историю
      this.addToHistory({
        ...this.getWindowData(),
        success: false,
        error: error.message,
      });
    } finally {
      // Восстанавливаем кнопку
      button.disabled = false;
      button.innerHTML = originalText;
    }
  }

  getWindowData() {
    const windowNumber = document.getElementById("windowNumber").value;
    const status = document.querySelector('input[name="status"]:checked').value;

    return {
      windowNumber: windowNumber,
      status: status === "free" ? 0 : 1,
    };
  }

  showResult(type, message) {
    const resultElement = document.getElementById("lastResult");
    const icon =
      type === "success" ? "fa-check-circle" : "fa-exclamation-circle";
    const color = type === "success" ? "#10b981" : "#ef4444";

    resultElement.innerHTML = `
            <i class="fas ${icon}" style="color: ${color}"></i>
            <span>${message}</span>
        `;

    // Показываем отправленные данные
    const windowData = this.getWindowData();
    console.log("Отправленные данные:", windowData);
  }

  addToHistory(item) {
    this.history.unshift(item);

    // Сохраняем только последние 10 записей
    if (this.history.length > 10) {
      this.history = this.history.slice(0, 10);
    }

    // Сохраняем в localStorage
    localStorage.setItem("windowHistory", JSON.stringify(this.history));

    // Обновляем отображение
    this.loadHistory();
  }

  loadHistory() {
    const historyList = document.getElementById("historyList");
    historyList.innerHTML = "";

    this.history.forEach((item) => {
      const historyItem = document.createElement("div");
      historyItem.className = `history-item ${
        item.success ? "success" : "error"
      }`;

      const icon = item.success ? "fa-check-circle" : "fa-exclamation-circle";
      const statusText = item.status === 0 ? "Свободно" : "Занято";

      historyItem.innerHTML = `
                <i class="fas ${icon}"></i>
                <span>${item.windowNumber}: ${statusText}</span>
            `;

      historyList.appendChild(historyItem);
    });
  }
}

// Инициализация приложения
document.addEventListener("DOMContentLoaded", () => {
  new WindowService();

  // Имитация подключения к RabbitMQ
  const rabbitStatus = document.getElementById("rabbitStatus");
  rabbitStatus.className = "status-connected";
  rabbitStatus.textContent = "Подключено";
});
