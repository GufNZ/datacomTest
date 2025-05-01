import React from 'react';
import { useNavigate, useParams } from 'react-router-dom';

import { Box, Typography, Button } from '@mui/material';

import { EditApplicationForm } from '../components/EditApplicationForm';

export const EditApplication: React.FC = () => {
	const navigate = useNavigate();
	const { id } = useParams<{ id: string }>();

	return id ? (
		<Box>
			<Typography variant="h4" component="h1" gutterBottom>
				Edit Job Application
			</Typography>
			<Box sx={{mb: 2}}>
				<Button
					variant="outlined"
					onClick={() => navigate(-1)}
					sx={{mr: 2}}
				>
					Back
				</Button>
			</Box>
			<EditApplicationForm id={id}/>
		</Box>
	) : <Typography color="error">Application ID not found</Typography>;
};
