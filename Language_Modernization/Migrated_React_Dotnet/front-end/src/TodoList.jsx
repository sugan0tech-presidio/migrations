import React, { useState, useEffect } from 'react';
import './TodoList.css';

const TodoList = () => {
    const [newTask, setNewTask] = useState('');
    const [todos, setTodos] = useState([]);
    const [name, setName] = useState(localStorage.getItem('name') || '');
    const [password, setPassword] = useState('');
    const [isRegistering, setIsRegistering] = useState(false);
    const [confirmPassword, setConfirmPassword] = useState('');

    useEffect(() => {
        if (name) {
            loadTodos();
        }
    }, [name]);

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
            body: JSON.stringify({ id: id }),
        });
        loadTodos();
    };

    const deleteTodo = async (id) => {
        await fetch(`http://localhost:5056/api/todo/${id}`, {
            method: 'DELETE',
        });
        loadTodos();
    };

    const handleLogin = async (e) => {
        e.preventDefault();
        const response = await fetch('http://localhost:5056/api/auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ name, password }),
        });

        if (response.ok) {
            localStorage.setItem('name', name);
            setPassword('');
            loadTodos();
        } else {
            alert('Login failed');
        }
    };

    const handleRegister = async (e) => {
        e.preventDefault();
        if (password !== confirmPassword) {
            alert('Passwords do not match');
            return;
        }

        const response = await fetch('http://localhost:5056/api/auth/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ name, password }),
        });

        if (response.ok) {
            alert('Registration successful! Please login.');
            setIsRegistering(false);
        } else {
            alert('Registration failed');
        }
    };

    const handleLogout = () => {
        localStorage.removeItem('name');
        setName('');
        setTodos([]);
    };

    if (!name) {
        return (
            <div className="content">
                {isRegistering ? (
                    <div>
                        <h3>Register</h3>
                        <form onSubmit={handleRegister} className="form-group">
                            <div className="form-group">
                                <label htmlFor="name">Name:</label>
                                <input
                                    id="name"
                                    className="form-control"
                                    value={name}
                                    onChange={(e) => setName(e.target.value)}
                                />
                            </div>
                            <div className="form-group">
                                <label htmlFor="password">Password:</label>
                                <input
                                    id="password"
                                    type="password"
                                    className="form-control"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                />
                            </div>
                            <div className="form-group">
                                <label htmlFor="confirmPassword">Confirm Password:</label>
                                <input
                                    id="confirmPassword"
                                    type="password"
                                    className="form-control"
                                    value={confirmPassword}
                                    onChange={(e) => setConfirmPassword(e.target.value)}
                                />
                            </div>
                            <button type="submit" className="btn btn-primary">Register</button>
                        </form>
                        <p>Already have an account? <button className="btn btn-link" onClick={() => setIsRegistering(false)}>Login here</button></p>
                    </div>
                ) : (
                    <div>
                        <h3>Login</h3>
                        <form onSubmit={handleLogin} className="form-group">
                            <div className="form-group">
                                <label htmlFor="name">Name:</label>
                                <input
                                    id="name"
                                    className="form-control"
                                    value={name}
                                    onChange={(e) => setName(e.target.value)}
                                />
                            </div>
                            <div className="form-group">
                                <label htmlFor="password">Password:</label>
                                <input
                                    id="password"
                                    type="password"
                                    className="form-control"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                />
                            </div>
                            <button type="submit" className="btn btn-primary">Login</button>
                        </form>
                        <p>Don't have an account? <button className="btn btn-link" onClick={() => setIsRegistering(true)}>Register here</button></p>
                    </div>
                )}
            </div>
        );
    }

    return (
        <div className="content">
            <h3>Todo List</h3>
            <button onClick={handleLogout} className="btn btn-secondary">Logout</button>

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