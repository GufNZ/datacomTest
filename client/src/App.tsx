import React from 'react';
import { BrowserRouter as Router, Link, Route, Routes } from 'react-router-dom';

import { AppBar, Button, Container, Toolbar, Typography } from '@mui/material';

import { AddApplication } from './pages/AddApplication';
import { EditApplication } from './pages/EditApplication';
import { Home } from './pages/Home';

function App() {
	return (
		<Router>
			<AppBar position="static">
				<Toolbar>
					<Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
						Job Application Tracker
					</Typography>
					<Button color="inherit" component={Link} to="/">
						Home
					</Button>
					<Button color="inherit" component={Link} to="/add">
						Add Application
					</Button>
				</Toolbar>
			</AppBar>
			<Container maxWidth="md" sx={{ mt: 8 }}>
				<Routes>
					<Route path="/" element={<Home/>}/>
					<Route path="/add" element={<AddApplication/>}/>
					<Route path="/edit/:id" element={<EditApplication/>}/>
				</Routes>
			</Container>
		</Router>
	);
}

export default App;
