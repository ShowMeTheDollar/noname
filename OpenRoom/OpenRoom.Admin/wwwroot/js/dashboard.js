document.addEventListener('DOMContentLoaded', function () {
    checkTokenAndRedirect();
});

function checkTokenAndRedirect() {
    const token = localStorage.getItem('token');

    if (!token) {
        // 如果 token 不存在，重定向到登入頁面
        window.location.href = '/Auth/login';
    }
}


async function fetchDataAndInitCharts() {
    try {
        const baseUrl = window.location.origin;
        const revenueUrl = `${baseUrl}/api/dashboard/monthly-revenue`;
        const ordersUrl = `${baseUrl}/api/dashboard/monthly-orders`;

        const revenueResponse = await axios.get(revenueUrl);
        const ordersResponse = await axios.get(ordersUrl);

        const revenueData = revenueResponse.data;
        const ordersData = ordersResponse.data;

        initCharts(revenueData, ordersData);
    } catch (error) {
        console.error('Failed to fetch data:', error);
    }
}

// 初始化圖表
function initCharts(revenueData, ordersData) {
    const revenueChart = echarts.init(document.getElementById('revenueChart'));
    const ordersChart = echarts.init(document.getElementById('ordersChart'));

    const revenueOption = {
        title: {
            text: '每月營收分布'
        },
        xAxis: {
            type: 'category',
            data: revenueData.map(item => `${item.month}月`),
        },
        yAxis: {
            type: 'value',
        },
        series: [
            {
                type: 'line',
                data: revenueData.map(item => item.revenue),
                smooth: true,
            },
        ],
    };

    const ordersOption = {
        title: {
            text: '每月訂單數量'
        },
        xAxis: {
            type: 'category',
            data: ordersData.map(item => `${item.month}月`),
        },
        yAxis: {
            type: 'value',
        },
        series: [
            {
                type: 'bar',
                data: ordersData.map(item => item.orders),
            },
        ],
    };

    revenueChart.setOption(revenueOption);
    ordersChart.setOption(ordersOption);
}

// 確保 DOM 已經完全加載
window.onload = function () {
    fetchDataAndInitCharts();
};