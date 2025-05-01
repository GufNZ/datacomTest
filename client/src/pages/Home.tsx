import React from 'react';
import { useNavigate } from 'react-router-dom';

import { Box, Typography } from '@mui/material';

import type { JobApplication } from '../types';
import { JobApplicationsList } from '../components/JobApplicationsList';

export const Home: React.FC = () => {
	const navigate = useNavigate();

	const handleEdit = (application: JobApplication) => {
		navigate(`/edit/${application.id}`);
	};

	return (
		<Box>
			<Typography variant="h4" component="h1" gutterBottom>
				Job Applications
			</Typography>
			<JobApplicationsList onEdit={handleEdit}/>
		</Box>
	);
};
