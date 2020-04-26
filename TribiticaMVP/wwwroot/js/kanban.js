// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const uri = 'api/v1/goals';
const default_id = '00000000-0000-0000-0000-000000000000';
let todos = { "day": [], "week": [], "year": [] };
let itemTypes = ['day', 'week', 'year'];

var Goal = function (id) {
    this.id = id;
    this.ownerId = getOwnerId(),
        this.header = '';
    this.description = '';
};

window.onload = function () {
    hideUpdateInput();
}

function getItems(itemType) {
    fetch(`${uri}/${itemType}`)
        .then(response => response.json())
        .then(data => _displayItems(data, itemType))
        .catch(error => console.error('Unable to get items.', error));
}

function addItem(itemType) {
    var headerTextElement = document.getElementById(`add-${itemType}-goal-name`);
    const headerText = headerTextElement.value.trim();

    const item = new Goal(default_id);
    item.header = headerText;

    fetch(`${uri}/${itemType}`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems(itemType);
            headerTextElement.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id, itemType) {
    fetch(`${uri}/${itemType}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getItems(itemType))
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id, itemType) {
    const item = todos[itemType].find(item => item.id === id);
    setUpdateDialogFields(item, itemType);
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const itemType = document.getElementById('edit-type').value;
    const item = {
        id: itemId,
        header: document.getElementById('edit-header').value.trim(),
        description: document.getElementById('edit-description').value.trim()
    };

    fetch(`${uri}/${itemType}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems(itemType))
        .catch(error => console.error('Unable to update item.', error));

    hideUpdateInput();

    return false;
}

function hideUpdateInput() {
    document.getElementById('editForm').style.display = 'none';
    setUpdateDialogFields(new Goal(default_id), 'nil');
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'to-do' : 'to-dos';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data, itemType) {
    const addArea = document.getElementById(`create-${itemType}-form`);
    let cloneArea = addArea.cloneNode(true);

    const tBody = document.getElementById(itemType);
    tBody.innerHTML = '';

    const button = document.createElement('button');
    tBody.appendChild(cloneArea);

    data.forEach(item => {
        let headerLabel = document.createElement('label');
        headerLabel.textContent = item.header;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm('${item.id}', '${itemType}')`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem('${item.id}', '${itemType}')`);

        let itemDiv = document.createElement('div');
        itemDiv.appendChild(headerLabel);
        itemDiv.appendChild(editButton);
        itemDiv.appendChild(deleteButton);
        tBody.appendChild(itemDiv);

        const existing = todos[itemType].find(i => i.id === item.id);
        if (existing != null) {
            // .remove done in js style
            const index = todos[itemType].indexOf(existing);
            if (index > -1) {
                todos[itemType].splice(index, 1);
            }
        }
        todos[itemType].push(item);
    });
}

function setUpdateDialogFields(goal, type) {
    document.getElementById('edit-id').value = goal.id;
    document.getElementById('edit-type').value = type;
    document.getElementById('edit-header').value = goal.header;
    document.getElementById('edit-description').value = goal.description;
}

function getOwnerId() {
    return document.getElementById('user-id').value.trim();
}