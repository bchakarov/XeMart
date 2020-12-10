(function () {
    var connection =
        new signalR.HubConnectionBuilder()
            .withUrl("/chat")
            .build();

    async function start() {
        try {
            await connection.start();
            await connection.invoke("LoadMessages");
        } catch (err) {
            console.error(err);
            setTimeout(start, 5000);
        }
    };

    connection.onclose(start);

    // Start the connection.
    start();

    var INDEX = 0;
    var submitButton = document.querySelector("#chat-submit");
    submitButton.addEventListener("click", async function (e) {
        e.preventDefault();
        var msg = $("#chat-input").val();
        if (msg.trim() == '') {
            return false;
        }
        await connection.invoke("Send", msg);
    });

    connection.on("NewMessage",
        function (roomMessages) {
            if (Array.isArray(roomMessages)) {
                $(".chat-logs").val('');
                for (var i = 0; i < roomMessages.length; i++) {
                    generate_message(roomMessages[i]);
                }
            } else {
                generate_message(roomMessages);
            }
        });

    function generate_message(msg) {
        INDEX++;
        var str = "";
        var type = "";
        if (msg.isByRoomOwner) {
            type = "self";
        } else {
            type = "user";
        }
        str += "<div id='cm-msg-" + INDEX + "' class=\"chat-msg " + type + "\">";
        str += "          <div class=\"cm-msg-text\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"XeMart\">";
        str += escapeHtml(msg.message);
        str += "          <\/div>";
        str += "        <\/div>";
        $(".chat-logs").append(str);
        $("#cm-msg-" + INDEX).hide().fadeIn(300);
        if (type == 'self') {
            $("#chat-input").val('');
        }
        $(".chat-logs").stop().animate({ scrollTop: $(".chat-logs")[0].scrollHeight }, 1000);
    }

    $("#chat-circle").click(function () {
        $("#chat-circle").toggle('scale');
        $(".chat-box").toggle('scale');
        $(".chat-logs").stop().animate({ scrollTop: $(".chat-logs")[0].scrollHeight }, 1000);
    })

    $(".chat-box-toggle").click(function () {
        $("#chat-circle").toggle('scale');
        $(".chat-box").toggle('scale');
    })

    function escapeHtml(unsafe) {
        return unsafe
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }
})();
