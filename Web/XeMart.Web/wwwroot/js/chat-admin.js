(async function chat() {
    var connection =
        new signalR.HubConnectionBuilder()
            .withUrl("/chat")
            .build();

    async function start() {
        try {
            await connection.start();
            await connection.invoke("LoadRooms");
        } catch (err) {
            console.error(err);
            setTimeout(start, 5000);
        }
    };

    connection.onclose(start);

    // Start the connection.
    start();

    connection.on("NewRoom",
        function (rooms) {
            if (Array.isArray(rooms)) {
                document.querySelector(".inbox_chat").innerHTML = "";
                for (var i = 0; i < rooms.length; i++) {
                    generate_room(rooms[i]);
                }
            } else {
                generate_room(rooms);
            }
        });

    connection.on("NewMessage",
        function (roomMessages) {
            if (Array.isArray(roomMessages)) {
                document.querySelector(".msg_history").innerHTML = "";
                for (var i = 0; i < roomMessages.length; i++) {
                    generate_message(roomMessages[i]);
                }
            } else {
                generate_message(roomMessages);
            }
        });

    var sendButton = document.querySelector(".msg_send_btn");
    sendButton.addEventListener("click", async function (e) {
        e.preventDefault();
        var msg = document.querySelector(".write_msg").value;
        if (msg.trim() == '') {
            return false;
        }

        $(".write_msg").val('');
        var roomId = document.querySelector(".active_chat").getAttribute("id");
        await connection.invoke("SendToRoom", roomId, msg);
    });

    function generate_room(room) {
        var str = "";
        str += "<div class=\"chat_list\"" + "id=\"" + room.id + "\">";
        str += "    <div class=\"chat_people\">";
        str += "        <div class=\"chat_ib\">";
        str += "            <h5>" + room.ownerUsername + " <span class=\"chat_date\"><time datetime=\"" + room.createdOn + "\"></time></span></h5>";
        str += "            <p>" + escapeHtml(room.lastMessage) + " </p>";
        str += "        </div>";
        str += "    </div>";
        str += "</div>";
        $(".inbox_chat").append(str);
        jQuery.parseTime();

        var newRoom = document.querySelector(`[id=\"${room.id}\"]`);
        newRoom.addEventListener("click", async function () {
            // Remove active_chat class from all chat_list elements before adding it to the clicked one
            var roomElements = document.querySelectorAll(".chat_list");
            for (var i = 0; i < roomElements.length; i++) {
                roomElements[i].classList.remove("active_chat");
            }

            newRoom.classList.add("active_chat");

            await connection.invoke("LoadRoomMessages", room.id);
        });
    }

    function generate_message(msg) {
        var str = "";
        if (msg.isByRoomOwner) {
            str += "<div class=\"incoming_msg\">";
            str += "    <div class=\"received_msg\">";
            str += "    <div class=\"received_withd_msg\">";
        } else {
            str += "<div class=\"outgoing_msg\">";
            str += "    <div class=\"sent_msg\">";
        }

        str += "        <p>" + escapeHtml(msg.message) + "</p>";
        str += "            <span class=\"time_date\"><time datetime=\"" + msg.createdOn + "\"></time>" + " By: " + msg.senderUsername + "</span>";
        str += "    </div>";
        str += "</div>";
        $(".msg_history").append(str);
        jQuery.parseTime();
        $(".msg_history").stop().animate({ scrollTop: $(".msg_history")[0].scrollHeight }, 1000);
    }

    function escapeHtml(unsafe) {
        if (unsafe == null) {
            return unsafe;
        }

        return unsafe
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }
})();
