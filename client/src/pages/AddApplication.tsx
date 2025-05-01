import React from 'react';
import { useNavigate } from 'react-router-dom';

import { Box, Button, Typography } from '@mui/material';

import { AddApplicationForm } from '../components/AddApplicationForm';

export const AddApplication: React.FC = () => {
	const navigate = useNavigate();

	return (
		<Box>
			<Typography variant="h4" component="h1" gutterBottom>
				Add New Job Application
			</Typography>
			<Box sx={{ mb: 2 }}>
				<Button
					variant="outlined"
					onClick={() => navigate(-1)}
					sx={{ mr: 2 }}
				>
					Back
				</Button>
			</Box>
			<AddApplicationForm/>
		</Box>
	);
};
