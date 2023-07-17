(function() {
    var steps = [
        function () { DemoRichEdit.commands.insertText.execute("Hello! This is new RichEdit API") },
        function () { DemoRichEdit.selection.selectAll() },
        function () { DemoRichEdit.commands.delete.execute() },
        function () { DemoRichEdit.commands.insertText.execute("Now you can do more.") },
        function () { DemoRichEdit.commands.insertParagraph.execute() },
        function () { DemoRichEdit.commands.insertText.execute("You can format text") },
        function () { DemoRichEdit.selection.goToPrevWord() },
        function () { DemoRichEdit.selection.goToPrevWord(true) },
        function () { DemoRichEdit.commands.changeFontFormatting.execute({ bold: true, foreColor: "#789" }) },
        function () { DemoRichEdit.selection.goToLineEnd() },
        function () { DemoRichEdit.commands.insertParagraph.execute() },
        function () { DemoRichEdit.commands.insertText.execute("and whole paragraphs") },
        function () { DemoRichEdit.commands.toggleParagraphAlignmentCenter.execute(true) },
        function () { DemoRichEdit.commands.insertParagraph.execute() },
        function () { DemoRichEdit.commands.toggleParagraphAlignmentCenter.execute(false) },
        function () { DemoRichEdit.commands.insertHeader.execute() },
        function () { DemoRichEdit.commands.insertText.execute("Insert headers...") },
        function () { DemoRichEdit.commands.closeHeaderFooter.execute() },
        function () { DemoRichEdit.selection.goToDocumentEnd() },
        function () { DemoRichEdit.commands.insertParagraph.execute() },
        function () { DemoRichEdit.commands.toggleBulletedList.execute() },
        function () { DemoRichEdit.commands.insertText.execute("and numbering lists ") },
        function () { DemoRichEdit.commands.insertParagraph.execute() },
        function () { DemoRichEdit.commands.toggleBulletedList.execute() },
        function () { DemoRichEdit.commands.insertTable.execute(2, 3) },
        function () { DemoRichEdit.commands.insertText.execute("and tables ") },
        function () { DemoRichEdit.selection.goToDocumentEnd() },
        function () { DemoRichEdit.commands.insertText.execute("and fields: ") },
        function () { DemoRichEdit.commands.createDateField.execute() },
        function () { DemoRichEdit.selection.selectAll() },
        function () { DemoRichEdit.commands.delete.execute() },
        function () { DemoRichEdit.commands.insertText.execute("Become a UI Superhero") },
        function () { DemoRichEdit.commands.insertParagraph.execute() },
        function () { DemoRichEdit.commands.insertPicture.execute(getImagePath()) },
    ];
    var currentStep = 0,
        currentTimer = -1;

    function startDemo() {
        var nextStep = function () {
            var step = steps[currentStep++];
            if (step) {
                DXLogger.executeAndLog(step);
                currentTimer = setTimeout(nextStep, 500);
            }
        };
        nextStep();
    }
    function restartDemo() {
        clearTimeout(currentTimer);
        currentStep = 0;
        DXLogger.executeAndLog(function () { DemoRichEdit.commands.fileNew.execute(); });
    }
    function getImagePath() {
        var path = "../Content/ui.png";
        return window.MVCx ? "../" + path : path;
    }

    window.startDemo = startDemo;
    window.restartDemo = restartDemo;
})();
