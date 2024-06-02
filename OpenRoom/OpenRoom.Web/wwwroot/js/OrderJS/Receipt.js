//window.jsPDF = window.jspdf.jsPDF;

//// Convert HTML content to PDF
//function Convert_HTML_To_PDF() {
//    const doc = new jsPDF();

//    // Source HTMLElement or a string containing HTML.
//    const elementHTML = document.querySelector(".receipt-details-container");

//    doc.html(elementHTML, {
//        callback: function (doc) {
//            // Save the PDF
//            doc.save('document-html.pdf');
//        },
//        margin: [10, 10, 10, 10],
//        autoPaging: 'text',
//        x: 0,
//        y: 0,
//        width: 190, //target width in the PDF document
//        windowWidth: 675 //window width in CSS pixels
//    });
//}



// PDF檔 -----------------------------------------------------------------
//document.getElementById('btnPdf').addEventListener('click', async function () {
//    var sHtml = document.querySelector('.receipt-details-container');

//    if (sHtml) {
//        sHtml = sHtml.innerHTML;

//        try {
//            Swal.fire({
//                icon: "info",
//                title: '下載中，請稍等...',
//                timerProgressBar: true,
//                didOpen: () => {
//                    Swal.showLoading();
//                },
//                allowOutsideClick: false,
//                allowEscapeKey: false,
//                allowEnterKey: false
//            });

//            const response = await fetch('/Order/GeneratePdf', {
//                method: 'POST',
//                headers: {
//                    'Content-Type': 'application/json'
//                },
//                body: JSON.stringify({ html: sHtml })
//            });

//            if (response.ok) {
//                const blob = await response.blob();
//                const url = window.URL.createObjectURL(blob);
//                const link = document.createElement('a');
//                link.href = url;
//                link.download = 'Receipt.pdf';
//                link.click();

//                // PDF下載完成後關閉彈跳視窗
//                Swal.close();
//            } else {
//                console.error('回應錯誤');
//                Swal.close();
//            }
//        } catch (error) {
//            console.error('Error:', error);
//            Swal.close();
//        }
//    } else {
//        console.error('找不到.receipt-details-container元素');
//        Swal.close();
//    }
//});

