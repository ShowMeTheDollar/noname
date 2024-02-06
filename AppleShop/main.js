let shopData = {};
let selectedModel, selectedColor, selectedStorage;

window.onload = function() {
    fetchMerchandise("./data/apple.json");
};

function fetchMerchandise(url) {
    fetch(url)
        .then((response) => response.json())
        .then((obj) => {
            renderingCategory(obj);
            return obj["iPhone 14 pro"];
        })
        .then((shop) => {
            shopData = shop;
            renderingShop(shopData);
        })
        .catch((err) => {
            console.warn(err); //在瀏覽器console中以警告信息的形式輸出
        });
}

function renderingCategory(merchandiseObj) {
    const categoryArray = Object.keys(merchandiseObj);
    categoryArray.forEach((category) => {
        const li = document.createElement("li");
        li.classList.add("nav-item");

        const button = document.createElement("button");
        button.classList.add("nav-link");
        button.textContent = category;
        button.addEventListener("click", function() {
            resetSummaryArea();
            shopData = merchandiseObj[category];
            renderingShop(shopData);
        });
        li.append(button);
        document.querySelector("#category_list").append(li);
    });
}

//重設小計區塊
function resetSummaryArea() {
    selectedModel = "";
    selectedColor = "";
    selectedStorage = "";
    createSummaryInfo();
}

function renderingShop(merchandise) {
    //console.log(merchandise);
    const priceArr = merchandise.specifications.map(spec => spec.price);
    const minPrice = Math.min(...priceArr); //直接將陣列傳遞，它會將整個陣列視為單一參數，而不是將陣列中的元素視為多個參數
    createTitleArea(merchandise.title, minPrice);
    const defaultImg = Object.values(merchandise.images).flat(); //flat陣列方法，可以將多層的嵌套陣列展平為一維陣列
    createCarousel(defaultImg);

    let widgetHTML = "";
    merchandise.widgets.forEach(widget => {
        widgetHTML += createWidgetHTML(widget);
    })
    document.querySelector(".spec-widget").innerHTML = widgetHTML;
    
    
}

function createTitleArea(title, price) {
    document.querySelector(".title-area").innerHTML = `
        <h1>
            ${title}
        </h1>
        <div class="total-price">
            NT$ ${price.toLocaleString()} 起
        </div>`;
};

function createCarousel(images) {
    const mainImgArea = document.querySelector(".main-img-area");
    const carouselIndicatorsHTML = createCarouselIndicatorsHTML(images);
    const carouselInnerHTML = createCarouselHTML(images);

    mainImgArea.innerHTML = `
        <div id="carouselExampleAutoplaying" class="carousel slide  sticky-top" data-bs-ride="carousel">
            <div class="carousel-indicators">
                ${carouselIndicatorsHTML}
            </div>
            <div class="carousel-inner">
                ${carouselInnerHTML}
            </div>
            <button class="carousel-control-prev" type="button" data-bs-target="#carouselExampleAutoplaying" data-bs-slide="prev">
                <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                <span class="visually-hidden">Previous</span>
            </button>
            <button class="carousel-control-next" type="button" data-bs-target="#carouselExampleAutoplaying" data-bs-slide="next">
                <span class="carousel-control-next-icon" aria-hidden="true"></span>
                <span class="visually-hidden">Next</span>
            </button>
        </div>`;
}

function createCarouselIndicatorsHTML(images) {
    let html = "";
    images.forEach((img, idx) => {
        html += `<button type="button" data-bs-target="#carouselExampleAutoplaying" data-bs-slide-to="${idx}"
        class="${idx === 0 ? 'active' : ''}" aria-current="true" aria-label="Slide ${idx + 1}"></button>`
    });
    return html;
}

function createCarouselHTML(images) {
    let html = "";
    images.forEach((img, idx) => {
        html += `<div class="carousel-item ${idx === 0 ? 'active' : ''}">
                    <img src="${img}" class="d-block w-100" alt="...">
                </div>`
    });
    return html;
}

function createWidgetHTML(widget) {
    let widgetKey = Object.keys(widget);
    let itemHTML = "";

    if (widgetKey[2] === "model") {
        widget[widgetKey[2]].forEach(model => {
            const specs = shopData.specifications.filter(spec => spec.model === model)
            const minPrice = Math.min(...specs.map(s => s.price));
            itemHTML += `
                <div class="col">
                    <div class="border border-secondary-subtle border-1 rounded-3 p-4 d-flex justify-content-between" role="button" onclick="clickHandler(this, '${widgetKey[2]}')" data-model="${model}">
                        <div>${model}</div>
                        <div>NT$ ${(minPrice).toLocaleString()} 起</div>
                    </div>
                </div>`
        });
    } else if (widgetKey[2] === "color") {
        widget[widgetKey[2]].forEach(colorObj => {
            const imgSize = widget.col > 2 ? 50 : 25;
            itemHTML += `
                <div class="col">
                    <div class="border border-secondary-subtle border-1 rounded-3 p-4 text-center" role="button" onclick="clickHandler(this, '${widgetKey[2]}')" data-color="${colorObj.colorCode}">
                        <img class="w-${imgSize}" src="${colorObj.colorImg}" alt="${colorObj.colorName}">
                    </div>
                </div>`
        });
    }else if (widgetKey[2] === "storage") {
        widget[widgetKey[2]].forEach(storage => {
            itemHTML += `
                <div class="col">
                    <div class="border border-secondary-subtle border-1 rounded-3 p-4 d-flex justify-content-between" role="button" onclick="clickHandler(this, '${widgetKey[2]}')" data-storage="${storage}">
                        <div>${storage}</div>
                        <div class="price"></div>
                    </div>
                </div>`
        });
    }

    let html = `
        <section class="widget-item mb-4 mx-lg-3">
            <h2 class="fs-4">${widget.title} <span class="text-black-50">${widget.subTitle}</span></h2>
            ${widgetKey[2] === "color" ? `<p><span class="picked-color fw-medium">顏色</span></p>` : ""}
            <div class="row row-cols-${widget.col} gy-3">
                ${itemHTML}
            </div>
        </section>`
    
    return html;
}

//WidgetItem點擊事件
function clickHandler(element, type) {
    //console.log(element); //傳入的 this 就是 e.target
    // console.log(element.dataset[type]);
    // console.log(element.getAttribute(`data-${type}`));
    //console.log(type);
    specSelectActiveHandler(element);

    const specWidget = document.querySelector(".spec-widget");
    if (type === "model") {
        selectedModel = element.dataset.model;
        //處理儲存裝置區價錢
        const specs = shopData.specifications.filter(s => s.model === selectedModel);
        specWidget.querySelectorAll("[data-storage]").forEach(el => {
            const spec = specs.find(s => s.storage === el.dataset.storage);
            el.querySelector(".price").textContent = `NT$ ${spec.price.toLocaleString()}`
        });
    }
    if (type === "color") {
        const color = shopData.widgets[1].color.find(c => c.colorCode === element.dataset.color);
        selectedColor = color.colorCode;

        //新增文字顏色
        specWidget.querySelector(".picked-color").textContent = `顏色 - ${color.colorName}`;
        //換主圖幻燈片
        const imgs = shopData.images[selectedColor];
        createCarousel(imgs);
    }

    if (type === "storage") {
        selectedStorage = element.dataset.storage;
    }

    createSummaryInfo();
}

//商品客製化區塊點擊動態樣式
function specSelectActiveHandler(element) {
    element.parentElement.parentElement.querySelectorAll('[role="button"]').forEach(el => {
        el.classList.remove("border-primary");
    });
    element.classList.remove("border-secondary-subtle");
    element.classList.add("border-primary");
}

//商品資訊小計區塊
function createSummaryInfo() {
    if (selectedModel && selectedColor && selectedStorage) {
        const spec = shopData.specifications.find(s => s.model === selectedModel && s.color === selectedColor && s.storage === selectedStorage);

        const price = spec.price.toLocaleString(); //可根據區域設置幣值的轉換顯示輸出
        const colorObj = shopData.widgets[1].color.find(c => c.colorCode === selectedColor);
        const title = `${selectedModel} ${selectedStorage} ${colorObj.colorName}`;
        document.querySelector(".summary-area .product-item").innerHTML = `
            <div class="title">${title}</div>
            <div class="price fw-bold">NT$ ${price}</div>`;
    }else {
        document.querySelector(".summary-area .product-item").innerHTML = "";
    }
}

