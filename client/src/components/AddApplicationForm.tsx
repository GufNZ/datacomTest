import React, { useState } from 'react';

import {
	Box,
	Button,
	TextField,
	FormControl,
	InputLabel,
	Select,
	MenuItem,
	Typography,
} from '@mui/material';

import { JobApplication } from '../types/JobApplication';
import { api } from '../services/api';
import { ApplicationStatus } from '../types/ApplicationStatus';

interface AddApplicationFormProps {
	onAdd: (application: JobApplication) => void;
}

export const AddApplicationForm: React.FC<AddApplicationFormProps> = ({ onAdd }) => {
	const [companyName, setCompanyName] = useState('');
	const [position, setPosition] = useState('');
	const [status, setStatus] = useState(ApplicationStatus.Applied);

	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault();
		try {
			const newApplication = {
				companyName,
				position,
				status: status as ApplicationStatus,
				dateApplied: new Date().toISOString().split('T')[0],
			};
			const response = await api.createApplication(newApplication);
			onAdd(response);
			setCompanyName('');
			setPosition('');
			setStatus(ApplicationStatus.Applied);
		} catch (error) {
			console.error('Error adding application:', error);
		}
	};

	return (
		<Box component="form" onSubmit={handleSubmit} sx={{ mt: 3 }}>
			<Typography variant="h6" gutterBottom>
				Add New Application
			</Typography>
			<TextField
				fullWidth
				label="Company Name"
				value={companyName}
				onChange={(e) => setCompanyName(e.target.value)}
				margin="normal"
				required
			/>
			<TextField
				fullWidth
				label="Position"
				value={position}
				onChange={(e) => setPosition(e.target.value)}
				margin="normal"
				required
			/>
			<FormControl fullWidth margin="normal">
				<InputLabel>Status</InputLabel>
				<Select
					value={status}
					onChange={(e) => setStatus(e.target.value as ApplicationStatus)}
					label="Status"
				>
					<MenuItem value={ApplicationStatus.Applied}>Applied</MenuItem>
					<MenuItem value={ApplicationStatus.Interview}>Interview</MenuItem>
					<MenuItem value={ApplicationStatus.Offer}>Offer</MenuItem>
					<MenuItem value={ApplicationStatus.Rejected}>Rejected</MenuItem>
				</Select>
			</FormControl>
			<Button
				type="submit"
				variant="contained"
				color="primary"
				fullWidth
				sx={{ mt: 2 }}
			>
				Add Application
			</Button>
		</Box>
	);
};
