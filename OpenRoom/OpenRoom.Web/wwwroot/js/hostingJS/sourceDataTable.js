
// 首先检查DataTable是否已经初始化，如果是，則销毁
if ($.fn.DataTable.isDataTable('#sourceDataTable')) {
    $('#sourceDataTable').DataTable().destroy();
}

// 然後重新初始化DataTable
$('#sourceDataTable').DataTable({
    language: {
        search: "搜尋:",// 修改搜尋框前的文字
        lengthMenu: "每頁顯示 _MENU_ 個記錄", // 修改每頁顯示多少個紀錄
        info: "顯示第 _START_ 至 _END_ 項，共 _TOTAL_ 項" // 修改表格左下角的訊息顯示
    }
    
});
