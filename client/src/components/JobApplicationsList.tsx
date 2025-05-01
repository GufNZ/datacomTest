import React from 'react';

import {
	Table,
	TableBody,
	TableCell,
	TableContainer,
	TableHead,
	TableRow,
	Paper,
	Box,
	IconButton,
	MenuItem,
	Menu,
} from '@mui/material';
import { Edit } from '@mui/icons-material';

import { api } from '../services/api';
import { JobApplication } from '../types/JobApplication';
import { ApplicationStatus } from '../types/ApplicationStatus';

interface JobApplicationsListProps {
	applications: JobApplication[];
	onEdit: (application: JobApplication) => void;
}

export const JobApplicationsList: React.FC<JobApplicationsListProps> = ({
	applications,
	onEdit,
}) => {
	const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
	const [selectedApplication, setSelectedApplication] = React.useState<JobApplication | null>(null);

	const handleEditClick = (event: React.MouseEvent<HTMLElement>, application: JobApplication) => {
		setAnchorEl(event.currentTarget);
		setSelectedApplication(application);
	};

	const handleEditClose = () => {
		setAnchorEl(null);
		setSelectedApplication(null);
	};

	const handleStatusChange = async (status: ApplicationStatus) => {
		if (selectedApplication) {
			const updatedApplication = { ...selectedApplication, status };
			await api.updateApplication(updatedApplication);
			onEdit(updatedApplication);
		}
		handleEditClose();
	};

	return (
		<Box sx={{ width: '100%' }}>
			<TableContainer component={Paper}>
				<Table>
					<TableHead>
						<TableRow>
							<TableCell>Company Name</TableCell>
							<TableCell>Position</TableCell>
							<TableCell>Status</TableCell>
							<TableCell>Date Applied</TableCell>
							<TableCell>Actions</TableCell>
						</TableRow>
					</TableHead>
					<TableBody>
						{applications.map((application) => (
							<TableRow key={application.id}>
								<TableCell>{application.companyName}</TableCell>
								<TableCell>{application.position}</TableCell>
								<TableCell>{application.status}</TableCell>
								<TableCell>{application.dateApplied}</TableCell>
								<TableCell>
									<IconButton
										size="small"
										onClick={(event) => handleEditClick(event, application)}
									>
										<Edit />
									</IconButton>
								</TableCell>
							</TableRow>
						))}
					</TableBody>
				</Table>
			</TableContainer>
			<Menu
				anchorEl={anchorEl}
				open={Boolean(anchorEl)}
				onClose={handleEditClose}
			>
				<MenuItem onClick={() => handleStatusChange(ApplicationStatus.Applied)}>Applied</MenuItem>
				<MenuItem onClick={() => handleStatusChange(ApplicationStatus.Interview)}>Interview</MenuItem>
				<MenuItem onClick={() => handleStatusChange(ApplicationStatus.Offer)}>Offer</MenuItem>
				<MenuItem onClick={() => handleStatusChange(ApplicationStatus.Rejected)}>Rejected</MenuItem>
			</Menu>
		</Box>
	);
};
