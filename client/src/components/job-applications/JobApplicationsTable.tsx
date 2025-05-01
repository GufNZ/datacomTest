import React from 'react';

import {
	IconButton,
	Menu,
	MenuItem,
	Paper,
	Stack,
	Table,
	TableBody,
	TableCell,
	TableContainer,
	TableHead,
	TableRow,
	TableSortLabel,
	styled,
} from '@mui/material';
import { Edit, Delete } from '@mui/icons-material';

import { api } from '../../services/api';
import { ApplicationStatus, type JobApplication, SortKey } from '../../types';

import type { TableColumn } from './types';

const StyledTableCell = styled(TableCell)({
	fontWeight: 'bold',
});

const headers: TableColumn[] = [
	{ id: SortKey.CompanyName, label: 'Company Name', align: 'left', sortable: true },
	{ id: SortKey.Position, label: 'Position', align: 'left', sortable: true },
	{ id: SortKey.Status, label: 'Status', align: 'left', sortable: true },
	{ id: SortKey.DateApplied, label: 'Date Applied', align: 'left', sortable: true },
	{ id: 'actions', label: 'Actions', align: 'center', sortable: false },
];

interface JobApplicationsTableProps {
	applications: JobApplication[];
	onEdit: (application: JobApplication) => void;
	onDelete: (application: JobApplication, callback?: () => void) => Promise<void>;
	sortConfig: { key: SortKey, direction: 'asc' | 'desc' };
	onSort: (key: SortKey) => void;
}

export const JobApplicationsTable = ({
	applications,
	onEdit,
	onDelete,
	sortConfig,
	onSort,
}: JobApplicationsTableProps): React.ReactElement => {
	const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
	const [selectedApplication, setSelectedApplication] = React.useState<JobApplication | null>(null);

	const handleEditClick = (application: JobApplication) => {
		onEdit(application);
		setAnchorEl(null);
	};

	const handleDeleteClick = async (application: JobApplication) => {
		setAnchorEl(null);
		await onDelete(application, () => {
			setSelectedApplication(null);
		});
	};

	const handleStatusChange = async (status: ApplicationStatus) => {
		if (selectedApplication) {
			try {
				const updatedApplication = { ...selectedApplication, status };
				await api.updateApplication(updatedApplication);
				setSelectedApplication(updatedApplication);
				onEdit(updatedApplication);
			} catch (error) {
				console.error('Error updating application status:', error);
			} finally {
				handleEditClose();
			}
		}
	};

	const handleEditClose = () => {
		setAnchorEl(null);
		setSelectedApplication(null);
	};

	const renderRow = (application: JobApplication) => {
		return (
			<TableRow>
				{headers.map((header) => (
					<TableCell key={header.id} align={header.align}>
						{header.id === 'actions' ? (
							<Stack direction="row" spacing={1}>
								<IconButton
									size="small"
									onClick={(e: React.MouseEvent) => {
										e.stopPropagation();
										handleEditClick(application);
									}}
								>
									<Edit/>
								</IconButton>
								<IconButton
									size="small"
									onClick={(e: React.MouseEvent) => {
										e.stopPropagation();
										handleDeleteClick(application);
									}}
									color="error"
								>
									<Delete/>
								</IconButton>
							</Stack>
						) : (
							application[header.id as keyof JobApplication]
						)}
					</TableCell>
				))}
			</TableRow>
		);
	};

	return (
		<>
			<TableContainer component={Paper}>
				<Table>
					<TableHead>
						<TableRow>
							{headers.map((header) => (
								header.sortable ? (
									<StyledTableCell key={header.id} align={header.align}>
										<TableSortLabel
											active={sortConfig.key === header.id}
											direction={sortConfig.key === header.id ? sortConfig.direction : 'asc'}
											onClick={() => onSort(header.id as SortKey)}
										>
											{header.label}
										</TableSortLabel>
									</StyledTableCell>
								) : (
									<StyledTableCell key={header.id} align={header.align}>
										{header.label}
									</StyledTableCell>
								)
							))}
						</TableRow>
					</TableHead>
					<TableBody>
						{applications.map((application) => renderRow(application))}
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
		</>
	);
};
