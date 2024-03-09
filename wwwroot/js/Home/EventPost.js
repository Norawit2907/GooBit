function CreateComment(_user_id, _user_image, _firstname, _lastname, _event_id)
    {
        const user_id = _user_id
        const user_image = _user_image
        const firstname = _firstname
        const lastname =  _lastname
        const event_id = _event_id
        const text = document.getElementById("create-comment-text").value
        const create_time = new Date();

        let new_comment = {
            user_id: user_id,
            user_image: user_image,
            firstname: firstname,
            lastname: lastname,
            event_id: event_id,
            text: text,
            create_time: create_time
        }
        let post = JSON.stringify(new_comment)
        const url = "http://localhost:5075/Home/CreateComment"
        let xhr = new XMLHttpRequest()
        
        xhr.open('POST', url, true)
        xhr.setRequestHeader('content-type', 'application/json; charset=UTF-8')
        xhr.send(post)

        xhr.onload = function () {
            if(xhr.status === 200) {
                window.alert("Comment Created Successfully")
            }
        }
    }

    function CreateReply(_comment_id, _user_id, _user_image, _firstname, _lastname)
    {
        const user_id = _user_id
        const user_image = _user_image
        const firstname = _firstname
        const lastname = _lastname
        const comment_id = _comment_id
        const create_time = new Date();
        const text = document.getElementById(_comment_id).value
        
        console.log(text);

        let new_reply = {
            user_id: user_id,
            firstname: firstname,
            lastname: lastname,
            user_image: user_image,
            comment_id: comment_id,
            text: text,
            create_time: create_time
        }
        console.log(new_reply)
        let post = JSON.stringify(new_reply)
        const url = "http://localhost:5075/Home/CreateReply"
        let xhr = new XMLHttpRequest()
        
        xhr.open('POST', url, true)
        xhr.setRequestHeader('content-type', 'application/json; charset=UTF-8')
        xhr.send(post)

        xhr.onload = function () {
            if(xhr.status === 200) {
                window.alert("Reply Created Successfully")
            }
        }
    }