var demoFeedbackServiceUrl = "https://js.devexpress.com/Feedback/Save/";
var DemoFeedbackPanelState = "DXDemoFeedbackPanelState";

function sendData(requestParams, doneCallback) {
    var xhr = new XMLHttpRequest();
    xhr.open('POST', demoFeedbackServiceUrl, true);

    var form_data = new FormData();
    form_data.append("value", requestParams.value);
    form_data.append("document", requestParams.document);
    form_data.append("project", requestParams.project);
    form_data.append("host", requestParams.host);
    if (requestParams.message)
        form_data.append("message", requestParams.message);

    xhr.send(form_data);
}

function onPositiveFeedbackClick(s, e) {
    showHideFeedbackDetails(true, true);
    sendData({ value: 1, document: window.location.pathname, project: "AspNet", host: window.location.host });
}

function onNegativeFeedbackClick(s, e) {
    showHideFeedbackDetails(true, false);
    sendData({ value: -1, document: window.location.pathname, project: "AspNet", host: window.location.host });
}

function onSendFeedbackClick(s, e) {
    var feedbackMessage = memoFeedbackText.GetText();

    if (feedbackMessage === "") return;
    var rate = isFeedbackPositive() ? 1 : -1;

    sendData({ value: rate, document: window.location.pathname, project: "AspNet", host: window.location.host, message: feedbackMessage });

    lblFeedbackCaption.SetText("We appreciate your feedback!");
    SetFeedbackLabelContainerSize("large");
    btnFeedbackClose.SetVisible(false);

    var mainСontainer = document.getElementById("feedbackContainerDiv");
    ASPx.RemoveClassNameFromElement(mainСontainer, "expandSlide");

    setTimeout(function () {
        showHideFeedbackDetails(false);
    }, 3000);
}

function onCloseFeedbackClick(s, e) {
    if (feedbackPanel.GetCollapsed())
        changeFeedbackPanelState(false, false, true);
    else
        showHideFeedbackDetails(false);
}

function onExpandPanelClick(s, e) {
    changeFeedbackPanelState(true, false, true);
}

function onFeedbackTextInput(s, e) {
    btnSendFeedback.SetEnabled(s.GetText() != "");
}

function onFeedbackTextInit(s, e) {
    ASPx.Attr.SetAttribute(s.GetInputElement(), "aria-label", "Message");
}

function onFeedbackPanelInit(s, e) {
    var currentState = ASPxClientUtils.GetCookie(DemoFeedbackPanelState);

    if (currentState != null) {
        var isVisible = currentState === "true";
        changeFullCompactMode(isVisible);
    }
    else {
        feedbackPanel.SetVisible(true);
    }
}

function showHideFeedbackDetails(isDetailVisible, isFeedbackPositive) {
    if (isDetailVisible) {
        var activeTab = pageControlfeedbackContent.GetTabByName(isFeedbackPositive ? "pagePositiveFeedback" : "pageNegativeFeedback");
        pageControlfeedbackContent.SetActiveTab(activeTab)
    }

    if (!isDetailVisible) {
        changeFeedbackPanelState(false, true, false);
        setTimeout(function () {
            updateContentVisibility(isDetailVisible)
        }, 500);
    }
    else {
        //animation
        updateContentVisibility(isDetailVisible)
        var mainСontainer = document.getElementById("feedbackContainerDiv");
        ASPx.AddClassNameToElement(mainСontainer, "expandSlide");
    }
}

function updateContentVisibility(isDetailVisible) {
    btnFeedbackPositive.SetVisible(!isDetailVisible);
    btnFeedbackNegative.SetVisible(!isDetailVisible);
    feedbackPanel.SetCollapsed(!isDetailVisible);

    btnFeedbackClose.SetVisible(true);
    memoFeedbackText.SetText("");
    btnSendFeedback.SetEnabled(false);

    if (!isDetailVisible) {
        lblFeedbackCaption.SetText("Did this demo address your needs?");
        SetFeedbackLabelContainerSize("small");
    }
    else {
        if (isFeedbackPositive()) {
            lblFeedbackCaption.SetText("EXCELLENT!!!");
            SetFeedbackLabelContainerSize("large");
        }
        else {
            lblFeedbackCaption.SetText("Our apologies...");
            SetFeedbackLabelContainerSize("large");
        }
    }
}

function changeFeedbackPanelState(isVisible, useAnimation, storeState) {
    if (storeState)
        ASPxClientUtils.SetCookie(DemoFeedbackPanelState, isVisible);

    var mainСontainer = document.getElementById("feedbackContainerDiv");
    if (useAnimation) {
        ASPx.AddClassNameToElement(mainСontainer, "collapseSlide");
        setTimeout(function () {
            ASPx.RemoveClassNameFromElement(mainСontainer, "expandSlide");    
            changeFullCompactMode(isVisible);
            ASPx.RemoveClassNameFromElement(mainСontainer, "collapseSlide");
        }, 500);
    }
    else {
        changeFullCompactMode(isVisible);
    }
}

function changeFullCompactMode(isVisible) { 
    var compactButtonContainer = document.getElementById("compactButtonContainer");
    if (!isVisible)
        ASPx.RemoveClassNameFromElement(compactButtonContainer, "collapsedElement");
    else
        ASPx.AddClassNameToElement(compactButtonContainer, "collapsedElement");

    feedbackPanel.SetVisible(isVisible);
}

function isFeedbackPositive() {
    var activeTab = pageControlfeedbackContent.GetActiveTab();
    return activeTab.name == "pagePositiveFeedback";
}

function SetFeedbackLabelContainerSize(size) {
    var lblMainElement = lblFeedbackCaption.GetMainElement();
    ASPx.RemoveClassNameFromElement(lblMainElement, "label-width-265");
    ASPx.RemoveClassNameFromElement(lblMainElement, "label-width-375");

    if (size == "small")
        ASPx.AddClassNameToElement(lblMainElement, "label-width-265");
    else if (size == "large")
        ASPx.AddClassNameToElement(lblMainElement, "label-width-375");
}
    