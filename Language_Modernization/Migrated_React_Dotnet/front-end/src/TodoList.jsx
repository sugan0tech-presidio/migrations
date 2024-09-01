import React, { useState, useEffect } from 'react';
import './TodoList.css';

const TodoList = () => {
    const [newTask, setNewTask] = useState('');
    const [todos, setTodos] = useState([]);

    useEffect(() => {
        loadTodos();
    }, []);

    const loadTodos = async () => {
        const response = await fetch('http://localhost:5056/api/todo');
        const data = await response.json();
        setTodos(data);
    };

    const addTodo = async (e) => {
        e.preventDefault();
        if (newTask) {
            await fetch('http://localhost:5056/api/todo', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ task: newTask }),
            });
            setNewTask('');
            loadTodos();
        }
    };

    const markAsCompleted = async (id) => {
        await fetch(`http://localhost:5056/api/todo/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ id: id }), // Ensure only the ID is sent to update
        });
        loadTodos();
    };

    const deleteTodo = async (id) => {
        await fetch(`http://localhost:5056/api/todo/${id}`, {
            method: 'DELETE',
        });
        loadTodos();
    };

    return (
        <div className="content">
            <h3>Todo List</h3>

            <form onSubmit={addTodo} className="form-group" name="addTodoForm">
                <div className="form-group">
                    <label htmlFor="task">Task:</label>
                    <input
                        id="task"
                        className="form-control"
                        value={newTask}
                        onChange={(e) => setNewTask(e.target.value)}
                    />
                </div>
                <button type="submit" className="btn btn-primary">Add Task</button>
            </form>

            <hr />

            <table className="table">
                <thead>
                    <tr>
                        <th>Task</th>
                        <th>Status</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {todos.map((todo) => (
                        <tr key={todo.id}>
                            <td>{todo.task}</td>
                            <td>{todo.isCompleted ? 'Completed' : 'Pending'}</td>
                            <td>
                                <button 
                                    className="btn btn-success" 
                                    onClick={() => markAsCompleted(todo.id)} 
                                    disabled={todo.isCompleted}>
                                    Complete
                                </button>
                                <button 
                                    className="btn btn-danger" 
                                    onClick={() => deleteTodo(todo.id)}>
                                    Delete
                                </button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default TodoList;