
chrome.runtime.onMessage.addListener((request, sender, sendResponse) => {
    // 進行異步操作
    someAsyncOperation().then(result => {
        sendResponse({ data: result });
    }).catch(error => {
        sendResponse({ error: error.message });
    });

    // 返回 true 表示你將會異步回應
    return true;
});
