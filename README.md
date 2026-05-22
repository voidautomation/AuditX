# AuditX – SAP ITGC Automation Tool

<p align="center">
  <img src="https://img.shields.io/badge/.NET-Framework%204.8-blue" />
  <img src="https://img.shields.io/badge/SAP-GUI-green" />
  <img src="https://img.shields.io/badge/Platform-Windows%2010%20%7C%2011-orange" />
  <img src="https://img.shields.io/badge/License-Enterprise-red" />
</p>

## Overview

AuditX is an enterprise-grade SAP IT General Controls (ITGC) automation solution designed to streamline audit execution, reporting, and compliance activities. The application automates SAP control testing, Word report generation, monthly consolidated reporting, and audit evidence collection.

The tool helps SAP Security teams, Audit teams, and Compliance teams execute controls efficiently while reducing manual effort and ensuring standardized audit practices.

---

## Purpose

This project defines and automates the Standard Operating Procedure (SOP) for SAP ITGC execution by:

- Standardizing audit execution
- Reducing manual effort
- Improving reporting accuracy
- Supporting compliance requirements
- Enabling centralized monthly reporting

---

## Scope

This solution applies to:

- SAP Security Teams
- ITGC Audit Teams
- Compliance & Risk Management Teams

Supported activities:

- Periodic control testing
- Individual control execution
- Bulk control execution
- Monthly reporting
- Audit evidence collection

---

# Features

✔ SAP GUI automation

✔ Single ITGC control execution

✔ Bulk execution of all controls

✔ Automated Microsoft Word report generation

✔ Monthly consolidated reporting

✔ Monthly CSV summary generation

✔ Dynamic configuration settings

✔ Exclusion user management

✔ License management

✔ SAP landscape import support

✔ Execution dashboard monitoring

✔ Detailed logging

✔ Foreground & Background execution modes

---

# System Requirements

## Operating System

- Windows 10
- Windows 11

> Active interactive session required for foreground execution and Microsoft Word Interop functionality.

---

## Software Requirements

| Component | Requirement |
|------------|-------------|
| .NET Framework | 4.8 |
| Visual C++ Redistributable | 2013 or later |
| Microsoft Word | Installed |
| SAP GUI | Installed |
| SAP GUI Scripting | Enabled |

---

## SAP Requirements

- SAP Logon executable accessible (`saplogon.exe`)
- Valid SAP credentials
- Appropriate authorization access
- Non-SSO login screen
- ITGCBOT credential-based execution

---

# Security Configuration

SAP GUI scripting security must be configured before using AuditX.

### Steps

1. Open SAP Logon
2. Navigate to:

```text
Options
    → Security
        → Security Settings
```

3. Add the AuditX application directory under:

```text
Always Allow
```

---

# Landscape Configuration

AuditX requires:

```text
SAPGUILandscape.XML
```

to exist in the same directory as the application executable.

If scripting is enabled only on a specific SAP instance:

1. Contact Basis Team
2. Identify scripting-enabled instance
3. Create SAP connection using that instance

---

# Licensing

A valid license file must exist within the application directory.

```text
AuditX/
│
├── AuditX.exe
├── License.lic
├── SAPGUILandscape.xml
```

---

# Login Procedure

## Administrator Login

### Step 1

Launch AuditX application

### Step 2

Enter:

- SAP Username
- SAP Password

### Step 3

Fill required details

### Step 4

Click:

```text
Generate License
```

### Step 5

Copy generated license file into application directory

---

## User Login

### Step 1

Click:

```text
Login
```

### Step 2

Enter Windows credentials

### Step 3

Click:

```text
OK
```

---

# Executing Single ITGC Control

Follow the steps below:

1. Select required Control ID
2. Select execution mode:

```text
Foreground
Background
```

3. Select:

```text
Single Execution
```

4. Select report month
5. Verify output path
6. Click:

```text
Execute
```

### Post Execution

- Wait for SAP execution to complete
- Review generated Word report
- Validate control status:

```text
Green → No findings
Red → Findings identified
```

- Review comments and evidence

---

# Executing All ITGC Controls

To execute all controls simultaneously:

1. Select:

```text
All Controls
```

2. Configure:

- Report month
- Output folder

3. Click:

```text
Execute
```

4. Monitor progress dashboard
5. Review generated reports

---

# Settings

The Settings module supports:

### User Exclusion Management

Add approved users separated by:

```text
user1;user2;user3
```

### Additional Configuration

- Reporting duration modification
- SAPGUILandscape.XML import
- ITGC Appendix import
- SAP Logon executable path setup

---

# Monthly Consolidated Report Generation

Steps:

1. Navigate to:

```text
Reporting
```

2. Select month

3. Click:

```text
Generate Monthly Report
```

Generated output:

```text
ITGC_Monthly_Report_{Month}.docx
```

Review:

- Red findings
- Comments
- Remediation status

---

# Output Structure

AuditX generates:

```text
AuditX Output
│
├── Individual Control Reports
│      ├── ITGC_001.docx
│      ├── ITGC_002.docx
│
├── Monthly Reports
│      ├── ITGC_Monthly_Report_Jan.docx
│
├── CSV Summary
│      ├── Summary_Jan.csv
│
└── Logs
       └── C:\Void\Logs
```

---

# Troubleshooting

| Issue | Resolution |
|---------|------------|
| SAP Attach Failure | Verify SAP GUI scripting enabled |
| Word Interop Error | Close all WINWORD processes |
| Blank Screenshots | Execute in Foreground mode |
| Execution Errors | Review logs |

Log location:

```text
C:\Void\Logs
```

---

# Auditor Responsibilities

Auditors are responsible for:

- Validating SAP data accuracy
- Ensuring correct Green/Red classification
- Providing detailed comments
- Archiving reports
- Escalating critical findings
- Following organizational compliance procedures

---

# Governance & Compliance

AuditX supports:

- SAP Security Governance
- IT General Controls (ITGC)
- Risk Management
- Compliance Monitoring
- Audit Evidence Retention

---

# Project Structure

```text
AuditX/
│
├── Application/
├── Reports/
├── Logs/
├── Config/
│   ├── SAPGUILandscape.xml
│   └── License.lic
│
├── Resources/
└── Documentation/
```

---

# Support

For technical issues:

**SAP Security Team**

or

**IT Automation Support Team**

---

# Disclaimer

This tool is intended for internal SAP ITGC audit execution and reporting purposes only. Users are responsible for ensuring compliance with organizational policies and access governance requirements.

---

© AuditX Enterprise Suite
