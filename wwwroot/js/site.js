// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function generateTimestamp(format = "ISO") {
    const now = new Date();

    switch (format.toUpperCase()) {
        case "ISO":
            return now.toISOString(); // e.g., "2025-03-06T10:15:30.000Z"
        case "UTC":
            return now.toUTCString(); // e.g., "Thu, 06 Mar 2025 10:15:30 GMT"
        case "LOCAL":
            return now.toLocaleString(); // e.g., "3/6/2025, 3:15:30 PM"
        case "EPOCH":
            return now.getTime(); // e.g., 1743748530000 (milliseconds since January 1, 1970)
        default:
            throw new Error("Invalid format. Supported formats: ISO, UTC, LOCAL, EPOCH.");
    }
}

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


function generateGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (char) {
        const random = Math.random() * 16 | 0;
        const value = char === 'x' ? random : (random & 0x3 | 0x8);
        return value.toString(16);
    });
}

// Example Usage:
// createItem('user', { name: 'John Doe', age: 30 });
// readItem('user');
// updateItem('user', { name: 'John Doe', age: 31 });
// deleteItem('user');
// clearAll();

function bucketizePoints(basePoints, n) {
    if (n <= 0 || basePoints.length === 0) {
        throw new Error("Invalid input values");
    }

    basePoints.sort((a, b) => a - b); // Ensure base points are sorted
    const min = basePoints[0];
    const max = basePoints[basePoints.length - 1];
    const step = (max - min) / n;
    const buckets = [];

    for (let i = 0; i < n; i++) {
        const from = min + i * step;
        const to = min + (i + 1) * step;
        buckets.push({
            from, to, points: [], color: 'rgba(68, 170, 213, 0.1)',
            label: {
                text: 'X',
                style: {
                    color: '#DDDDDD'
                }
            }
        });
    }

    // Assign points to buckets
    basePoints.forEach(point => {
        for (let bucket of buckets) {
            if (point >= bucket.from && point < bucket.to) {
                bucket.points.push(point);
                break;
            }
        }
    });

    return buckets;
}

// Example usage:
// console.log(bucketizePoints([1, 5, 10, 15, 20, 25, 30], 3));

function getCheckedValues() {
    const checkedValues = [];
    $('.form-check-input:checked').each(function () {
        checkedValues.push($(this).attr('id'));
    });
    return checkedValues;
}

function listToCSV(strings) {
    return strings
        .map(str => {
            if (str.includes('"') || str.includes(',')) {
                return `"${str.replace(/"/g, '""')}"`;
            }
            return str;
        })
        .join(',');
}

function getCheckedValuesCSV() {
    return listToCSV(getCheckedValues());
}
