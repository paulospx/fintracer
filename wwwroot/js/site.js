// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// Function to create or add data to localStorage
function createItem(key, value) {
    if (!key || !value) {
        console.error('Key and value are required.');
        return;
    }

    localStorage.setItem(key, JSON.stringify(value));
    console.log(`Item with key "${key}" has been created.`);
}

// Function to read or retrieve data from localStorage
function readItem(key) {
    if (!key) {
        console.error('Key is required.');
        return null;
    }

    const item = localStorage.getItem(key);
    if (item) {
        console.log(`Item retrieved:`, JSON.parse(item));
        return JSON.parse(item);
    } else {
        console.warn(`No item found with key "${key}".`);
        return null;
    }
}

// Function to update an existing item in localStorage
function updateItem(key, newValue) {
    if (!key || !newValue) {
        console.error('Key and new value are required.');
        return;
    }

    if (localStorage.getItem(key)) {
        localStorage.setItem(key, JSON.stringify(newValue));
        console.log(`Item with key "${key}" has been updated.`);
    } else {
        console.warn(`Cannot update. No item found with key "${key}".`);
    }
}

// Function to delete an item from localStorage
function deleteItem(key) {
    if (!key) {
        console.error('Key is required.');
        return;
    }

    if (localStorage.getItem(key)) {
        localStorage.removeItem(key);
        console.log(`Item with key "${key}" has been deleted.`);
    } else {
        console.warn(`No item found with key "${key}" to delete.`);
    }
}

// Function to clear all items in localStorage
function clearAll() {
    localStorage.clear();
}

// Example Usage:
// createItem('user', { name: 'John Doe', age: 30 });
// readItem('user');
// updateItem('user', { name: 'John Doe', age: 31 });
// deleteItem('user');
// clearAll();
