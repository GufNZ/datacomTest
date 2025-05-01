import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router } from 'react-router-dom';

import { Container, Paper, Typography, Box } from '@mui/material';

import { JobApplicationsList } from './components/JobApplicationsList';
import { AddApplicationForm } from './components/AddApplicationForm';
import { api } from './services/api';
import { JobApplication } from './types/JobApplication';

function App() {
	const [applications, setApplications] = useState<JobApplication[]>([]);

	useEffect(() => {
		const fetchApplications = async () => {
			try {
				const data = await api.getApplications();
				setApplications(data);
			} catch (error) {
				console.error('Error fetching applications:', error);
			}
		};

		fetchApplications();
	}, []);

	const handleAdd = (newApplication: JobApplication) => {
		setApplications([...applications, newApplication]);
	};

	const handleEdit = (updatedApplication: JobApplication) => {
		setApplications(
			applications.map(app =>
				app.id === updatedApplication.id ? updatedApplication : app
			)
		);
	};

	return (
		<Router>
			<Container maxWidth="md" sx={{ mt: 4 }}>
				<Paper elevation={3} sx={{ p: 3 }}>
					<Typography variant="h4" component="h1" gutterBottom>
						Job Application Tracker
					</Typography>
					<Box sx={{ mb: 4 }}>
						<AddApplicationForm onAdd={handleAdd}/>
					</Box>
					<JobApplicationsList
						applications={applications}
						onEdit={handleEdit}
					/>
				</Paper>
			</Container>
		</Router>
	);
}

export default App;
