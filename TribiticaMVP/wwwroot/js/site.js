// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const uri = 'api/Goals';
const default_id = '00000000-0000-0000-0000-000000000000';
let todos = [];
var Goal = function (id) {
    this.id = id;
    this.ownerId = document.getElementById('user-id').value.trim(),
    this.header = '';
    this.description = '';
};

window.onload = function () {
    hideUpdateInput();

}

function getItems() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
    const headerYearly = document.getElementById('add-yearly-name');

    const item = new Goal(default_id);
    item.header = headerYearly.value.trim();

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems();
            headerYearly.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = todos.find(item => item.id === id);
    setUpdateDialogFields(item);
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: itemId,
        header: document.getElementById('edit-header').value.trim(),
        description: document.getElementById('edit-description').value.trim()
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    hideUpdateInput();

    return false;
}

function hideUpdateInput() {
    document.getElementById('editForm').style.display = 'none';
    setUpdateDialogFields(new Goal(default_id));
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'to-do' : 'to-dos';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const addArea = document.getElementById('create-year-form');
    let cloneArea = addArea.cloneNode(true);

    const tBody = document.getElementById('year');
    tBody.innerHTML = '';

    const button = document.createElement('button');
    tBody.appendChild(cloneArea);

    data.forEach(item => {
        let headerLabel = document.createElement('label');
        headerLabel.textContent = item.header;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm('${item.id}')`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem('${item.id}')`);

        let itemDiv = document.createElement('div');
        //let tr = tBody.insertRow();

        //let td1 = tr.insertCell(0);
        itemDiv.appendChild(headerLabel);

        //let td3 = tr.insertCell(2);
        itemDiv.appendChild(editButton);

        //let td4 = tr.insertCell(3);
        itemDiv.appendChild(deleteButton);

        tBody.appendChild(itemDiv);
    });

    todos = data;
}

function setUpdateDialogFields(goal) {
    document.getElementById('edit-id').value = goal.id;
    document.getElementById('edit-header').value = goal.header;
    document.getElementById('edit-description').value = goal.description;
}