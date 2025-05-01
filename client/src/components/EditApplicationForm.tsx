import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import {
	Button,
	Box,
	FormControl,
	InputLabel,
	MenuItem,
	Select,
	TextField,
	Typography,
} from '@mui/material';

import { api } from '../services/api';
import { ApplicationStatus, type JobApplication } from '../types';

interface EditApplicationFormProps {
	id: string;
}

export const EditApplicationForm: React.FC<EditApplicationFormProps> = ({ id }) => {
	const [companyName, setCompanyName] = useState('');
	const [position, setPosition] = useState('');
	const [status, setStatus] = useState(ApplicationStatus.Applied);
	const navigate = useNavigate();

	const [loading, setLoading] = useState(true);
	const [error, setError] = useState<string | null>(null);

	useEffect(() => {
		const fetchApplication = async () => {
			try {
				const application = await api.getApplication(parseInt(id));
				setCompanyName(application.companyName);
				setPosition(application.position);
				setStatus(application.status);
				setLoading(false);
			} catch (err) {
				setError('Failed to fetch application');
				console.error('Error fetching application:', err);
				setLoading(false);
			}
		};

		fetchApplication();
	}, [id]);

	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault();
		try {
			const updatedApplication: JobApplication = {
				id: parseInt(id),
				companyName,
				position,
				status: status as ApplicationStatus,
				dateApplied: new Date().toISOString().split('T')[0],
			};
			await api.updateApplication(updatedApplication);
			navigate('/');
		} catch (error) {
			console.error('Error updating application:', error);
		}
	};

	if (loading) {
		return <Typography>Loading...</Typography>;
	}

	if (error) {
		return <Typography color="error">{error}</Typography>;
	}

	return (
		<Box component="form" onSubmit={handleSubmit} sx={{ mt: 3 }}>
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
				Update Application
			</Button>
		</Box>
	);
};
